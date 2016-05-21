using System.Linq;

using Lyralei.Core.UserManager.Models;
using Lyralei.Core.ServerQueryConnection.Models;
using Lyralei.TS3_Objects.Entities;
using System;

namespace Lyralei.Core.PermissionManager
{
    public class PermissionManager : Base.CoreBase, Base.ICore
    {
        ServerQueryConnection.ServerQueryConnection ServerQueryConnection;

        //private TS3QueryLib.Core.Server.Entities.Permission Permissions

        #region Standards
        public PermissionManager(Subscribers Subscriber) : base(Subscriber)
        {
            CoreDependencies.Add(typeof(ServerQueryConnection.ServerQueryConnection).Name);
        }

        public void UserInitialize(CoreList CoreList)
        {
            ServerQueryConnection = (ServerQueryConnection.ServerQueryConnection)CoreList[typeof(ServerQueryConnection.ServerQueryConnection).Name];

            //TODO: Add permission list from ts3 somehow
            //var PermissionsList = ServerQueryConnection.QueryRunner.GetPermissionList().ToList();
        }

        public CommandRuleSets DefineCommandSchemas()
        {
            return null;
        }
        #endregion

        #region CRUD
        public void CreatePermission(Users User, Props.Permission Permission)
        {
            using (var db = new CoreContext())
            {
                Models.UserPermissions permission = new Models.UserPermissions()
                {
                    PermissionName = Permission.Name(),
                    PermissionValue = Permission.Value(),
                    UserId = User.UserId,
                    Users = User
                };

                db.UserPermissions.Add(permission);
                db.SaveChanges();
            }
        }

        public void DeletePermission(Users User, Props.Permission Permission)
        {
            using (var db = new CoreContext())
            {
                var permSearch = db.UserPermissions.SingleOrDefault(x => x.UserId == User.UserId && x.PermissionName == Permission.Name());

                if (permSearch != null)
                {
                    db.UserPermissions.Remove(permSearch);
                    db.SaveChanges();
                }
            }
        }

        public Props.Permission GetPermission(Users User, Props.Permission Permission, bool includeNativePermissions = false)
        {

            using (var db = new CoreContext())
            {
                var permSearch = db.UserPermissions.Single(x => x.UserId == User.UserId && x.PermissionName == Permission.Name());
                return new Props.Permission(permSearch.PermissionName, permSearch.PermissionValue);
            }
        }

        public void UpdatePermission(Users User, Props.Permission Permission)
        {
            using (var db = new CoreContext())
            {
                var permSearch = db.UserPermissions.SingleOrDefault(x => x.UserId == User.UserId && x.PermissionName == Permission.Name());
                permSearch.PermissionValue = Permission.Value();

                db.SaveChanges();
            }
        }
        #endregion


    }
}
