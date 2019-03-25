using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using Ziri.BLL.SYS;
using Ziri.MDL;
using Ziri.DAL;

namespace Ziri.BLL.ITEM
{
    public class Spec
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "Spec", Title = "规格", IconFont = "fa fa-th-large", Enabled = true };
        public static List<ActionInfo> ActionInfos = new List<ActionInfo> {
            new ActionInfo{ Name = "Init", Title = "初始化", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "List", Title = "列表", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Create", Title = "创建", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Modify", Title = "修改", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Enabled", Title = "启用", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "Disabled", Title = "禁用", IconFont = "" , Enabled = true},
        };

        //初始化信息
        public static void InitSpecInfo(out AlertMessage alertMessage)
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
                var delCount = EF.Database.ExecuteSqlCommand("TRUNCATE TABLE SpecInfo; TRUNCATE TABLE SpecValue;");

                //规格
                var specInfos = new List<SpecInfo> {
                    new SpecInfo{ IconFont = "flaticon2-gift", Name = "Color", Title = "颜色", Enabled = true },
                    new SpecInfo{ IconFont = "flaticon2-gift", Name = "Memory", Title = "内存", Enabled = true },
                    new SpecInfo{ IconFont = "flaticon2-gift", Name = "Clothing sizes", Title = "服装尺寸", Enabled = true },
                };
                EF.SpecInfos.AddRange(specInfos);
                EF.SaveChanges();

                //参数
                List<SpecValue> specValues = new List<SpecValue>();
                foreach (var specInfo in specInfos)
                {
                    switch (specInfo.Name)
                    {
                        case "Color":
                            specValues.AddRange(new List<SpecValue> {
                                new SpecValue{ SpecID = specInfo.ID, Value = "黑", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "白", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "红", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "橙", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "黄", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "绿", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "青", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "蓝", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "紫", Enabled = true },
                            });
                            break;
                        case "Memory":
                            specValues.AddRange(new List<SpecValue> {
                                new SpecValue{ SpecID = specInfo.ID, Value = "32G", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "64G", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "128G", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "256G", Enabled = true },
                            });
                            break;
                        case "Clothing sizes":
                            specValues.AddRange(new List<SpecValue> {
                                new SpecValue{ SpecID = specInfo.ID, Value = "M", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "L", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "XL", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "XXL", Enabled = true },
                                new SpecValue{ SpecID = specInfo.ID, Value = "XXXL", Enabled = true },
                            });
                            break;
                    }
                }
                EF.SpecValues.AddRange(specValues);
                EF.SaveChanges();
            }
        }

        //更新信息
        public static SpecInfo SpecInfoUpload(SpecInfo specInfo, List<string> specValues, out AlertMessage alertMessage)
        {
            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (specInfo.ID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //表单检查
            if (string.IsNullOrWhiteSpace(specInfo.Name))
            {
                alertMessage = new AlertMessage { Message = "规格代码不能为空。", Type = AlertType.warning };
                return null;
            }
            if (string.IsNullOrWhiteSpace(specInfo.Title))
            {
                alertMessage = new AlertMessage { Message = "规格名称不能为空。", Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                //修改是否存在？规格代码、名称唯一？
                SpecInfo spec_exist = null;
                SpecInfo spec_name_exist = null;
                SpecInfo spec_title_exist = null;
                if (specInfo.ID == 0)
                {
                    spec_name_exist = EF.SpecInfos.Where(i => i.Name == specInfo.Name).FirstOrDefault();
                    spec_title_exist = EF.SpecInfos.Where(i => i.Title == specInfo.Title).FirstOrDefault();
                }
                else
                {
                    spec_exist = EF.SpecInfos.Where(i => i.ID == specInfo.ID).FirstOrDefault();
                    if (spec_exist == null)
                    {
                        alertMessage = new AlertMessage { Message = string.Format("规格编号[{0}]不存在。", specInfo.ID), Type = AlertType.warning };
                        return null;
                    }
                    spec_name_exist = EF.SpecInfos.Where(i => i.ID != specInfo.ID && i.Name == specInfo.Name).FirstOrDefault();
                    spec_title_exist = EF.SpecInfos.Where(i => i.ID != specInfo.ID && i.Title == specInfo.Title).FirstOrDefault();
                }
                if (spec_name_exist != null && spec_name_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("规格代码[{0}]已被ID[{1}]使用。", specInfo.Name, spec_name_exist.ID), Type = AlertType.warning };
                    return null;
                }
                if (spec_title_exist != null && spec_title_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("规格名称[{0}]已被ID[{1}]使用。", specInfo.Title, spec_title_exist.ID), Type = AlertType.warning };
                    return null;
                }

                //数据保存
                using (TransactionScope TS = new TransactionScope())
                {
                    //规格信息
                    if (specInfo.ID == 0) { spec_exist = EF.SpecInfos.Add(new SpecInfo { Enabled = true }); }
                    spec_exist.Name = specInfo.Name;
                    spec_exist.Title = specInfo.Title;
                    spec_exist.IconFont = specInfo.IconFont;
                    EF.SaveChanges();

                    //规格参数
                    foreach (var item in specValues)
                    {
                        var specValue_exist = EF.SpecValues.Where(i => i.SpecID == spec_exist.ID && i.Value == item).FirstOrDefault();
                        if (specValue_exist == null)
                        {
                            EF.SpecValues.Add(specValue_exist = new SpecValue { SpecID = spec_exist.ID, Value = item, Enabled = true });
                        }
                        EF.SaveChanges();
                    }
                    TS.Complete();
                }

                //更新完成
                alertMessage = null;
                return spec_exist;
            }
        }

        //获取列表规格列表
        public static List<SpecInfo> GetSpecInfos(out AlertMessage alertMessage, bool Enabled = false)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "List").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //返回列表
            using (var EF = new EF())
            {
                if (Enabled)
                {
                    return EF.SpecInfos.Where(i => i.Enabled == true).ToList();
                }
                return EF.SpecInfos.ToList();
            }
        }

        //获取列表规格参数列表
        public static List<SpecValue> GetSpecValues(long SpecID, out AlertMessage alertMessage, bool Enabled = false)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "List").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //返回列表
            using (var EF = new EF())
            {
                if (Enabled)
                {
                    return EF.SpecValues.Where(i => i.SpecID == SpecID && i.Enabled == true).ToList();
                }
                return EF.SpecValues.Where(i => i.SpecID == SpecID).ToList();
            }
        }

        //获取修改规格信息
        public static SpecInfo GetModifySpecInfo(long SpecID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //返回列表
            using (var EF = new EF())
            {
                return EF.SpecInfos.Where(i => i.ID == SpecID).FirstOrDefault();
            }
        }

        //获取列表规格参数列表
        public static List<SpecValue> GetModifySpecValues(long SpecID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //返回列表
            using (var EF = new EF())
            {
                return EF.SpecValues.Where(i => i.SpecID == SpecID).ToList();
            }
        }

        //设置规格状态
        public static void SetSpecEnabled(long SpecID, bool Enabled, out AlertMessage alertMessage)
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
                var specInfo = EF.SpecInfos.Where(i => i.ID == SpecID).FirstOrDefault();
                specInfo.Enabled = Enabled;
                EF.SaveChanges();

                alertMessage = new AlertMessage { Message = "规格[" + specInfo.Title + "]" + (Enabled ? "启用" : "禁用") + "成功！", Type = AlertType.success };
            }
        }

        //设置规格参数状态
        public static void SetValueEnabled(long ValueID, bool Enabled, out AlertMessage alertMessage)
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
                var valueInfo = EF.SpecValues.Where(i => i.ID == ValueID).FirstOrDefault();
                valueInfo.Enabled = Enabled;
                EF.SaveChanges();

                alertMessage = new AlertMessage { Message = "规格参数[" + valueInfo.Value + "]" + (Enabled ? "启用" : "禁用") + "成功！", Type = AlertType.success };
            }
        }
    }
}
