namespace Ziri.MDL
{
    //模块用户授权
    public class ModuleActionUserAuth
    {
        public long ID { get; set; }
        public string IconFont { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public long UserID { get; set; }
        public bool UserAuth { get; set; }
    }

    //模块角色授权
    public class ModuleActionRoleAuth
    {
        public long ID { get; set; }
        public string IconFont { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public long RoleID { get; set; }
        public bool RoleAuth { get; set; }
    }
}
