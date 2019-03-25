using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Ziri.MDL;
using Ziri.DAL;
using Ziri.BLL.LIST;

namespace Ziri.BLL.SYS
{
    public class Module
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "Module", Title = "模块", IconFont = "fa fa-microchip", Enabled = true };
        public static List<ActionInfo> ActionInfos = new List<ActionInfo> {
            new ActionInfo{ Name = "Init", Title = "初始化", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "List", Title = "列表", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Modify", Title = "修改", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Details", Title = "详情", IconFont = "", Enabled = true },
        };

        //初始化
        public static void InitModuleInfo(out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Init").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return;
            }

            using (var EF = new EF())
            {
                //清除数据表
                var delCount = EF.Database.ExecuteSqlCommand(@"TRUNCATE TABLE ModuleAction; TRUNCATE TABLE ActionInfo; TRUNCATE TABLE ModuleInfo;");

                //反射查询模块信息
                try
                {
                    foreach (Type tClass in Assembly.GetExecutingAssembly().GetTypes())
                    {
                        ModuleInfo moduleInfo = null;
                        List<ActionInfo> actionInfos = null;
                        FieldInfo[] tCFields = tClass.GetFields();
                        foreach (FieldInfo field in tCFields)
                        {
                            switch (field.Name)
                            {
                                case "ModuleInfo": moduleInfo = field.GetValue(null) as ModuleInfo; break;
                                case "ActionInfos": actionInfos = field.GetValue(null) as List<ActionInfo>; break;
                            }
                            if (moduleInfo != null && actionInfos != null)
                            {
                                EF.ModuleInfos.Add(moduleInfo);
                                EF.ActionInfos.AddRange(actionInfos);
                                EF.SaveChanges();
                                foreach (var item in actionInfos)
                                {
                                    EF.ModuleActions.Add(new ModuleAction
                                    {
                                        ModuleID = moduleInfo.ID,
                                        ActionID = item.ID,
                                    });
                                }
                                EF.SaveChanges();
                                break;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
            }
        }

        //获取列表
        public static List<ModuleInfo> GetModuleInfos(List<ListFilterField> FilterField, List<ListOrderField> OrderField
            , int PageSize, int PageIndex, out long TotalRowCount, out AlertMessage alertMessage)
        {
            TotalRowCount = 0;
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "List").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                IEnumerable<ModuleInfo> DataList = from moduleInfos in EF.ModuleInfos select moduleInfos;

                //筛选
                foreach (var item in FilterField)
                {
                    if (item.Name == "NameAndTitle" && item.Value.Count > 0)
                    {
                        var predicate = PredicateExtensions.False<ModuleInfo>();    //设置为False，所有and条件都应该放在or之后，如where (type=1 or type=14) and status==0
                        foreach (var t in item.Value)
                        {
                            var KWPart = t.ToLower();
                            switch (item.CmpareMode)
                            {
                                case FilterCmpareMode.Equal:
                                    predicate = predicate.Or(p => p.Name.ToLower() == KWPart || p.Title.ToLower() == KWPart);
                                    break;
                                case FilterCmpareMode.Like:
                                    predicate = predicate.Or(p => p.Name.ToLower().Contains(KWPart) || p.Title.ToLower().Contains(KWPart));
                                    break;
                            }
                        }
                        DataList = DataList.Where(predicate.Compile());
                    }
                }

                //排序
                if (OrderField.Count == 0) { DataList = DataList.OrderByDescending(i => i.ID); }
                else
                {
                    foreach (var item in OrderField)
                    {
                        switch (item.Mode)
                        {
                            case OrderByMode.Asc:
                                DataList = from list in DataList
                                           orderby OrderBy.GetPropertyValue(list, item.Name)
                                           select list;
                                break;
                            case OrderByMode.Desc:
                                DataList = from list in DataList
                                           orderby OrderBy.GetPropertyValue(list, item.Name) descending
                                           select list;
                                break;
                        }
                    }
                }

                //分页
                TotalRowCount = DataList.Count();
                if (TotalRowCount == 0) { return null; }
                int PageCount = (int)Math.Ceiling((double)TotalRowCount / PageSize);
                if (PageIndex > PageCount) { PageIndex = PageCount; }
                return DataList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
        }

        //获取修改
        public static ModuleInfo GetModuleModify(long ModuleID, out AlertMessage alertMessage)
        {
            return GetModuleInfo(ModuleID, "Modify", out alertMessage);
        }

        //获取详细
        public static ModuleInfo GetModuleDetails(long ModuleID, out AlertMessage alertMessage)
        {
            return GetModuleInfo(ModuleID, "Details", out alertMessage);
        }

        //获取模块信息，不存在则新增
        internal static ModuleInfo GetModuleInfo(ModuleInfo ModuleInfo)
        {
            using (var EF = new EF())
            {
                var moduleInfo = ModuleInfo.ID == 0
                    ? EF.ModuleInfos.Where(i => i.Name == ModuleInfo.Name).FirstOrDefault()
                    : EF.ModuleInfos.Where(i => i.ID == ModuleInfo.ID).FirstOrDefault();
                if (moduleInfo == null)
                {
                    EF.ModuleInfos.Add(moduleInfo = new ModuleInfo
                    {
                        Name = ModuleInfo.Name,
                        Title = ModuleInfo.Title,
                        IconFont = ModuleInfo.IconFont,
                        Enabled = true
                    });
                    EF.SaveChanges();
                }
                return moduleInfo;
            }
        }

        //获取模块的操作信息
        public static List<ActionInfo> GetModuleActionInfo(long ModuleID)
        {
            using (var EF = new EF())
            {
                return (from moduleAction in EF.ModuleActions
                        join actionInfo in EF.ActionInfos on moduleAction.ActionID equals actionInfo.ID
                        where moduleAction.ModuleID == ModuleID
                        select actionInfo
                        ).ToList();
            }
        }

        //获取模块的操作信息，不存在则新增
        internal static ActionInfo GetModuleActionInfo(long ModuleID, ActionInfo ActionInfo)
        {
            using (var EF = new EF())
            {
                ActionInfo moduleActionInfo = ActionInfo.ID == 0
                    ? (from moduleAction in EF.ModuleActions
                       join actionInfo in EF.ActionInfos on moduleAction.ActionID equals actionInfo.ID
                       where moduleAction.ModuleID == ModuleID && actionInfo.Name == ActionInfo.Name
                       select actionInfo).FirstOrDefault()
                    : (from moduleAction in EF.ModuleActions
                       join actionInfo in EF.ActionInfos on moduleAction.ActionID equals actionInfo.ID
                       where moduleAction.ModuleID == ModuleID && actionInfo.ID == ActionInfo.ID
                       select actionInfo).FirstOrDefault();
                if (moduleActionInfo == null)
                {
                    ActionInfo actionInfo = null;
                    if (ActionInfo.ID == 0) { EF.ActionInfos.Where(i => i.Name == ActionInfo.Name).FirstOrDefault(); }
                    else { EF.ActionInfos.Where(i => i.ID == ActionInfo.ID).FirstOrDefault(); }
                    if (actionInfo == null)
                    {
                        EF.ActionInfos.Add(actionInfo = new ActionInfo
                        {
                            Name = ActionInfo.Name,
                            Title = ActionInfo.Title,
                            IconFont = ActionInfo.IconFont,
                            Enabled = true
                        });
                        EF.SaveChanges();
                    }
                    EF.ModuleActions.Add(new ModuleAction
                    {
                        ModuleID = ModuleID,
                        ActionID = actionInfo.ID
                    });
                    EF.SaveChanges();
                    moduleActionInfo = actionInfo;
                }

                return moduleActionInfo;
            }
        }

        //更新模块信息
        public static ModuleInfo ModuleInfoUpload(ModuleInfo moduleInfo, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                var module_exist = EF.ModuleInfos.Where(i => i.ID == moduleInfo.ID).FirstOrDefault();
                if (module_exist == null) { return null; }
                module_exist.Title = moduleInfo.Title;
                module_exist.IconFont = moduleInfo.IconFont;
                EF.SaveChanges();
                return module_exist;
            }
        }

        //获取模块信息
        private static ModuleInfo GetModuleInfo(long ModuleID, string ActionName, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == ActionName).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                return EF.ModuleInfos.Where(i => i.ID == ModuleID).FirstOrDefault();
            }
        }
    }
}
