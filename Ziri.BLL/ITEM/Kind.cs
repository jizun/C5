using System.Collections.Generic;
using System.Linq;
using Ziri.BLL.SYS;
using Ziri.MDL;
using Ziri.DAL;

namespace Ziri.BLL.ITEM
{
    public class Kind
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "Kind", Title = "品类", IconFont = "fa fa-th-large", Enabled = true };
        public static List<ActionInfo> ActionInfos = new List<ActionInfo> {
            new ActionInfo{ Name = "Init", Title = "初始化", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "List", Title = "列表", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Details", Title = "详情", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Create", Title = "创建", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Modify", Title = "修改", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Enabled", Title = "启用", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "Disabled", Title = "禁用", IconFont = "" , Enabled = true},
        };

        //初始化信息
        public static void InitKindInfo(out AlertMessage alertMessage)
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
                var delCount = EF.Database.ExecuteSqlCommand("TRUNCATE TABLE KindInfo;");

                //根类
                var kindInfos = new List<KindInfo> {
                    new KindInfo{ OrderBy = 1, Name = "K001", Title="运动健康", Enabled = true },
                    new KindInfo{ OrderBy = 2, Name = "K002", Title="智能影音", Enabled = true },
                    new KindInfo{ OrderBy = 3, Name = "K003", Title="电子美容", Enabled = true },
                    new KindInfo{ OrderBy = 4, Name = "K004", Title="电子阅读", Enabled = true },
                    new KindInfo{ OrderBy = 5, Name = "K005", Title="功能配件", Enabled = true },
                };
                EF.KindInfos.AddRange(kindInfos);
                EF.SaveChanges();
                //子类
                List<KindInfo> kindL1s = null;
                foreach (var kindRoot in kindInfos)
                {
                    switch (kindRoot.Name)
                    {
                        case "K001":
                            kindL1s = new List<KindInfo> {
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 1,  Name = kindRoot.Name + "-001", Title = "智能手表", Enabled = true },
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 2,  Name = kindRoot.Name + "-002", Title = "智能手环", Enabled = true },
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 3,  Name = kindRoot.Name + "-003", Title = "智能按摩仪", Enabled = true },
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 4,  Name = kindRoot.Name + "-004", Title = "智能无人机", Enabled = true },
                            };
                            break;
                        case "K002":
                            kindL1s = new List<KindInfo> {
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 1,  Name = kindRoot.Name + "-001", Title = "智能耳机", Enabled = true },
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 2,  Name = kindRoot.Name + "-002", Title = "智能音箱", Enabled = true },
                            };
                            break;
                        case "K003":
                            kindL1s = new List<KindInfo> {
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 1,  Name = kindRoot.Name + "-001", Title = "脱毛仪", Enabled = true },
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 2,  Name = kindRoot.Name + "-002", Title = "吹风机", Enabled = true },
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 3,  Name = kindRoot.Name + "-003", Title = "洁面仪", Enabled = true },
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 4,  Name = kindRoot.Name + "-004", Title = "美颜仪", Enabled = true },
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 5,  Name = kindRoot.Name + "-005", Title = "补水仪", Enabled = true },
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 6,  Name = kindRoot.Name + "-006", Title = "化妆镜", Enabled = true },
                            };
                            break;
                        case "K004":
                            kindL1s = new List<KindInfo> {
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 1,  Name = kindRoot.Name + "-001", Title = "阅读器", Enabled = true },
                            };
                            break;
                        case "K005":
                            kindL1s = new List<KindInfo> {
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 1,  Name = kindRoot.Name + "-001", Title = "保护壳", Enabled = true },
                                new KindInfo{ ParentID = kindRoot.ID, OrderBy = 2,  Name = kindRoot.Name + "-002", Title = "移动电源", Enabled = true },
                            };
                            break;
                    }
                    EF.KindInfos.AddRange(kindL1s);
                    EF.SaveChanges();
                }
            }
        }

        //获取品类根列表
        public static List<KindInfo> GetKindRootInfos(out AlertMessage alertMessage, bool Enabled = false)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "List").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //查询
            return GetKindInfos(0, Enabled);
        }

        //获取品类信息
        public static List<KindInfo> GetKindInfos(long ParentID, bool Enabled = false)
        {
            using (var EF = new EF())
            {
                if (Enabled)
                {
                    return EF.KindInfos.Where(i => i.Enabled == true && i.ParentID == ParentID).OrderBy(i => i.OrderBy).ToList();
                }
                return EF.KindInfos.Where(i => i.ParentID == ParentID).OrderBy(i => i.OrderBy).ToList();
            }
        }

        //获取修改
        public static KindInfo GetModifyInfo(long KindID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (KindID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetKindInfo(KindID);
        }

        //更新信息
        public static KindInfo KindInfoUpload(KindInfo kindInfo, out AlertMessage alertMessage)
        {
            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (kindInfo.ID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //表单检查
            if (string.IsNullOrWhiteSpace(kindInfo.Name))
            {
                alertMessage = new AlertMessage { Message = "品类代码不能为空。", Type = AlertType.warning };
                return null;
            }
            if (string.IsNullOrWhiteSpace(kindInfo.Title))
            {
                alertMessage = new AlertMessage { Message = "品类名称不能为空。", Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                //修改是否存在？代码、名称唯一？
                KindInfo kind_exist = null;
                KindInfo kind_name_exist = null;
                KindInfo kind_title_exist = null;
                if (kindInfo.ID == 0)
                {
                    kind_name_exist = EF.KindInfos.Where(i => i.Name == kindInfo.Name).FirstOrDefault();
                    kind_title_exist = EF.KindInfos.Where(i => i.Title == kindInfo.Title).FirstOrDefault();
                }
                else
                {
                    kind_exist = EF.KindInfos.Where(i => i.ID == kindInfo.ID).FirstOrDefault();
                    if (kind_exist == null)
                    {
                        alertMessage = new AlertMessage { Message = string.Format("品牌编号[{0}]不存在。", kindInfo.ID), Type = AlertType.warning };
                        return null;
                    }
                    kind_name_exist = EF.KindInfos.Where(i => i.ID != kindInfo.ID && i.Name == kindInfo.Name).FirstOrDefault();
                    kind_title_exist = EF.KindInfos.Where(i => i.ID != kindInfo.ID && i.Title == kindInfo.Title).FirstOrDefault();
                }
                if (kind_name_exist != null && kind_name_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("品牌代码[{0}]已被ID[{1}]使用。", kindInfo.Name, kind_name_exist.ID), Type = AlertType.warning };
                    return null;
                }
                if (kind_title_exist != null && kind_title_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("品牌名称[{0}]已被ID[{1}]使用。", kindInfo.Title, kind_title_exist.ID), Type = AlertType.warning };
                    return null;
                }

                //数据保存
                if (kindInfo.ID == 0) { kind_exist = EF.KindInfos.Add(new KindInfo { Enabled = true, }); }
                kind_exist.ParentID = kindInfo.ParentID;
                kind_exist.OrderBy = kindInfo.OrderBy;
                kind_exist.Name = kindInfo.Name;
                kind_exist.Title = kindInfo.Title;
                kind_exist.LogoFileID = kindInfo.LogoFileID;
                EF.SaveChanges();

                //更新完成
                alertMessage = null;
                return kind_exist;
            }
        }

        //获取详情
        public static KindInfo GetDetailsInfo(long KindID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Details").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetKindInfo(KindID);
        }

        //获取产品列表
        public static List<GoodsInfo> GetGoodsInfos(long KindID)
        {
            using (var EF = new EF())
            {
                return (from kindInfo in EF.KindInfos
                        join goodsKind in EF.GoodsKinds on kindInfo.ID equals goodsKind.KindID
                        join goodsInfo in EF.GoodsInfos on goodsKind.GoodsID equals goodsInfo.ID
                        where kindInfo.ID == KindID
                        select goodsInfo).ToList();
            }
        }

        //设置状态
        public static void SetKindEnabled(long KindID, bool Enabled, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (Enabled ? "Enabled" : "Disabled")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return;
            }

            using (var EF = new EF())
            {
                var kindInfo = EF.KindInfos.Where(i => i.ID == KindID).FirstOrDefault();
                kindInfo.Enabled = Enabled;
                EF.SaveChanges();

                alertMessage = new AlertMessage { Message = "品类[" + kindInfo.Title + "]" + (Enabled ? "启用" : "禁用") + "成功！", Type = AlertType.success };
            }
        }

        //获取信息
        private static KindInfo GetKindInfo(long KindID)
        {
            using (var EF = new EF())
            {
                return EF.KindInfos.Where(i => i.ID == KindID).FirstOrDefault();
            }
        }
    }
}
