using PLNFramework.Security;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PLNFramework.Security.ViewModels;
using PLNFramework.Security.Repositories;
using PLNFramework.Security.Menus;
using PLNFramework.Security.Models;
using Microsoft.Owin.Security;
using PLNFramework.Security.Filters;

namespace PLNFramework.Security.Controllers
{
    [AuthorizeResource(ActionKey = "ACCESS", ResourceKey = "SECURITY_SETUP")]
    public class SecuritySettingsController : Controller
    {
        internal static IMapper mapper;

        static SecuritySettingsController()
        {
            var config = new MapperConfiguration(x =>
            {

                x.CreateMap<AppUserViewModel, AppUser>().ReverseMap();

                x.CreateMap<EditAppUserViewModel, AppUser>().ReverseMap();

                x.CreateMap<AppRoleViewModel, AppRole>().ReverseMap();

                x.CreateMap<EditAppRoleViewModel, AppRole>().ReverseMap();

                x.CreateMap<NewAppRoleViewModel, AppRole>().ReverseMap();

                x.CreateMap<AppPermission, AppPermissionViewModel>()
                .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => src.Action.Name))
                .ForMember(dest => dest.ActionKey, opt => opt.MapFrom(src => src.Action.Key))
                .ForMember(dest => dest.ResourceName, opt => opt.MapFrom(src => src.Resource.Name))
                .ForMember(dest => dest.ResourceKey, opt => opt.MapFrom(src => src.Resource.Key));

                x.CreateMap<AppPermissionViewModel, AppPermission>();

                x.CreateMap<DetailsAppRoleViewModel, AppRole>().ReverseMap();

                x.CreateMap<MenuViewModel, AppMenu>().ReverseMap();

                x.CreateMap<NewMenuViewModel, AppMenu>().ReverseMap();

                x.CreateMap<EditMenuViewModel, AppMenu>().ReverseMap();

                x.CreateMap<MenuItemViewModel, AppMenuItem>().ReverseMap();

                x.CreateMap<NewMenuItemViewModel, AppMenuItem>()
                .ForMember(dest => dest.AppMenuKey, opt => opt.MapFrom(src => src.MenuKey)).ReverseMap();

                x.CreateMap<EditMenuItemViewModel, AppMenuItem>()
                .ForMember(dest => dest.AppMenuKey, opt => opt.MapFrom(src => src.MenuKey)).ReverseMap();

                x.CreateMap<AppAction, AppActionViewModel>().ReverseMap();
                x.CreateMap<AppResource, AppResourceViewModel>().ReverseMap();

                x.CreateMap<AppUserPermission, AppUserPermissionViewModel>()
                .ForMember(dest => dest.ActionKey, opt => opt.MapFrom(src => src.Permission.Action.Key))
                .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => src.Permission.Action.Name))
                .ForMember(dest => dest.ResourceKey, opt => opt.MapFrom(src => src.Permission.Resource.Key))
                .ForMember(dest => dest.ResourceName, opt => opt.MapFrom(src => src.Permission.Resource.Name));

            });

            mapper = config.CreateMapper();
        }
        private AppUserManager _userManager;
        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        #region Users
        public ActionResult Users()
        {
            AppSecurityContext context = new AppSecurityContext();
            var users = UserManager.Users;
            var model = mapper.Map<IEnumerable<AppUserViewModel>>(users);
            return View(model);
        }

        public ActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateUser(AppUserViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = new AppUser { Id = Guid.NewGuid().ToString(), UserName = model.Email, Email = model.Email, EmailConfirmed= true};

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users", "SecuritySettings");
                }
                AddErrors(result);
            }
            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }

        public ActionResult EditUser(string id)
        {

            var context = new AppSecurityContext();
            var rolRepository = new RoleRepository(context);
            var userRolRepository = new UserRoleRepository(context);

            var user = context.Users.Find(id);
            var model = new EditAppUserViewModel();
            model.Email = user.Email;
            model.Id = user.Id;

            var roles = rolRepository.GetAll();

            var assignedRoles = userRolRepository.GetAssignedUserRoles(id);

            if (assignedRoles.Count() > 0)
            {
                model.SelectedRoles = assignedRoles.Select(x => x.RoleId).ToArray();
            }
            else
            {
                model.SelectedRoles = new string[0];
            }
            model.AvailableRoles = mapper.Map<ICollection<AppRoleViewModel>>(roles);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditUser(string id, EditAppUserViewModel model)
        {

            var context = new AppSecurityContext();
            var rolRepository = new RoleRepository(context);
            var userRolRepository = new UserRoleRepository(context);

            if (ModelState.IsValid)
            {
                //Se asignan los roles
                var user = mapper.Map<AppUser>(model);
                var assignedRoles = userRolRepository.GetAssignedUserRoles(id);
                var selectedRoles = new List<AppRole>();
                if (model.SelectedRoles != null)
                {
                    foreach (var rolId in model.SelectedRoles)
                    {
                        selectedRoles.Add(new AppRole { Id = rolId });
                    }
                }
                userRolRepository.UpdateUserWithRoles(user, selectedRoles);
                try
                {
                    context.SaveChanges();
                }
                catch
                {
                    return View(model);
                }
                return RedirectToAction("Users");
            }

            var roles = rolRepository.GetAll();
            model.AvailableRoles = mapper.Map<ICollection<AppRoleViewModel>>(roles);
            return View(model);
        }



        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        public ActionResult DetailsUser(string id)
        {
            var context = new AppSecurityContext();
            var userRepository = new UserRepository(context);
            var roleRepository = new RoleRepository(context);
            var userPermissionRep = new UserPermissionRepository(context);

            var user = userRepository.Find(id);
            var roles = roleRepository.GetRolesByUserId(user.Id);
            var model = new DetailsAppUserViewModel();
            model.Email = user.Email;
            model.Id = user.Id;
            model.AssignedRoles = new List<AppRoleViewModel>();
            foreach (var item in roles)
            {
                model.AssignedRoles.Add(new AppRoleViewModel { Id = item.Id, Name = item.Name });
            }
            var permissions = userPermissionRep.GetAll()
                        .Where(x => x.UserId == id);
            model.UserPermissions = mapper.Map<IEnumerable<AppUserPermissionViewModel>>(permissions);
            return View(model);
        }

        public ActionResult Actions()
        {
            using (var context = new AppSecurityContext())
            {
                var repository = new ActionRepository(context);
                var actions = repository.GetAll();
                var model = mapper.Map<IEnumerable<AppActionViewModel>>(actions);
                return View(model);
            }
        }

        public ActionResult CreateOrUpdateAction(string id)
        {
            var model = new AppActionViewModel();
            if (id != null)
            {

                using (var context = new AppSecurityContext())
                {
                    var repository = new ActionRepository(context);
                    var action = repository.Find(id);
                    model = mapper.Map<AppActionViewModel>(action);
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateOrUpdateAction(string id, AppActionViewModel model)
        {

            using (var context = new AppSecurityContext())
            {
                try
                {
                    var action = mapper.Map<AppAction>(model);
                    var repository = new ActionRepository(context);
                    if (id != null)
                    {
                        repository.Update(action);
                    }
                    else
                    {
                        repository.Insert(action);
                    }
                    context.SaveChanges();
                    return RedirectToAction("Actions");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(model);
                }
            }
        }


        public ActionResult Resources()
        {
            using (var context = new AppSecurityContext())
            {
                var repository = new ResourceRepository(context);
                var resources = repository.GetAll();
                var model = mapper.Map<IEnumerable<AppResourceViewModel>>(resources);
                return View(model);
            }
        }

        public ActionResult CreateOrUpdateResource(string id)
        {
            var model = new AppResourceViewModel();
            if (id != null)
            {
                using (var context = new AppSecurityContext())
                {
                    var repository = new ResourceRepository(context);
                    var resource = repository.Find(id);
                    model = mapper.Map<AppResourceViewModel>(resource);
                    return View(model);

                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateOrUpdateResource(string id, AppResourceViewModel model)
        {
            using (var context = new AppSecurityContext())
            {
                try
                {
                    var resource = mapper.Map<AppResource>(model);
                    var repository = new ResourceRepository(context);
                    if (id != null)
                    {
                        repository.Update(resource);
                    }
                    else
                    {
                        repository.Insert(resource);
                    }
                    context.SaveChanges();
                    return RedirectToAction("Resources");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(model);
                }
            }
        }



        public ActionResult Permissions()
        {
            using (var context = new AppSecurityContext())
            {
                var repository = new PermissionRepository(context);
                var permissions = repository.GetAll();
                var model = mapper.Map<IEnumerable<AppPermissionViewModel>>(permissions);
                return View(model);
            }
        }



        public ActionResult CreatePermission()
        {

            var model = new NewAppPermissionViewModel();
            using (var context = new AppSecurityContext())
            {
                var actionRepository = new ActionRepository(context);
                var actions = actionRepository.GetAll().OrderBy(x => x.Name);

                model.AvailableActions = mapper.Map<ICollection<AppActionViewModel>>(actions);
                model.ResourcesList = PopulateResourcesForNewPermission(model.SelectedResource);
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult CreatePermission(NewAppPermissionViewModel model)
        {
            using (var context = new AppSecurityContext())
            {
                var actionRepository = new ActionRepository(context);
                var resourceRepository = new ResourceRepository(context);
                var permissionRepository = new PermissionRepository(context);
                try
                {
                    var resource = resourceRepository.Find(model.SelectedResource);
                    var actions = actionRepository.Query(x => model.SelectedActions.Contains(x.Key));
                    foreach (var action in actions)
                    {
                        var permission = new AppPermission();
                        permission.Action = action;
                        permission.Resource = resource;
                        permissionRepository.Add(permission);
                    }
                    context.SaveChanges();
                    return RedirectToAction("Permissions");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    var actions = actionRepository.GetAll().OrderBy(x => x.Name);
                    model.AvailableActions = mapper.Map<ICollection<AppActionViewModel>>(actions);
                    model.ResourcesList = PopulateResourcesForNewPermission(model.SelectedResource);
                    return View(model);
                }
            }

        }

        public ActionResult EditPermission(string id)
        { //Clave del resource

            var model = new EditAppPermissionViewModel();

            using (var context = new AppSecurityContext())
            {
                var actionRepository = new ActionRepository(context);
                var resourceRepository = new ResourceRepository(context);
                var permissionRepository = new PermissionRepository(context);
                try
                {
                    var actions = actionRepository.GetAll();
                    var resource = resourceRepository.Find(id);
                    var permissions = permissionRepository.GetAll().Where(x => x.ResourceKey == resource.Key);
                    var actionKeys = permissions.Select(x => x.ActionKey).ToArray();
                    model.AvailableActions = mapper.Map<ICollection<AppActionViewModel>>(actions);
                    model.SelectedActions = actionKeys;
                    model.ResourceKey = resource.Key;
                    model.ResourceName = resource.Name;

                    return View(model);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(model);
                }
            }

        }

        [HttpPost]
        public ActionResult EditPermission(string id, EditAppPermissionViewModel model)
        { //Clave del resource

            using (var context = new AppSecurityContext())
            {
                var actionRepository = new ActionRepository(context);
                var resourceRepository = new ResourceRepository(context);
                var permissionRepository = new PermissionRepository(context);
                try
                {
                    var actions = actionRepository.GetAll();
                    var resource = resourceRepository.Find(id);
                    var permissions = permissionRepository.GetAll().Where(x => x.ResourceKey == resource.Key);
                    var actionKeys = permissions.Select(x => x.ActionKey).ToArray();
                    model.AvailableActions = mapper.Map<ICollection<AppActionViewModel>>(actions);

                    //Se eliminan los permisos anteriores
                    foreach (var perm in permissions)
                    {
                        permissionRepository.Delete(perm);
                    }

                    if (model.SelectedActions != null)
                    {
                        //Se agregan los nuevos
                        var actionsForInsert = actionRepository.Query(x => model.SelectedActions.Contains(x.Key));
                        foreach (var action in actionsForInsert)
                        {
                            var permission = new AppPermission();
                            permission.Action = action;
                            permission.Resource = resource;
                            permissionRepository.Add(permission);
                        }
                        model.SelectedActions = actionKeys;
                        model.ResourceKey = resource.Key;
                        model.ResourceName = resource.Name;
                    }
                    context.SaveChanges();
                    return RedirectToAction("Permissions");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    if (ex.InnerException != null)
                    {
                        ViewBag.Error += ex.InnerException.Message;
                    }
                    return View(model);
                }
            }

        }

        public ActionResult UserPermission(string id)
        {

            try
            {
                using (var context = new AppSecurityContext())
                {
                    var userPermissionRep = new UserPermissionRepository(context);
                    var permissionRep = new PermissionRepository(context);
                    var userRep = new UserRepository(context);

                    var permissions = userPermissionRep.GetAll()
                        .Where(x => x.UserId == id);

                    var user = userRep.Find(id);

                    var model = new ListAppUserPermissionViewModel();
                    model.UserId = user.Id;
                    model.UserName = user.UserName;
                    model.UserPermissions = mapper.Map<IEnumerable<AppUserPermissionViewModel>>(permissions);
                    return View(model);
                }
            }
            catch
            {
                return View("Users");
            }
        }

        public ActionResult CreateUserPermission(string id)
        {
            var model = new NewAppUserPermissionViewModel();

            using (var context = new AppSecurityContext())
            {
                var resourcesRepository = new ResourceRepository(context);
                var userPermissionRository = new UserPermissionRepository(context);
                var userRepository = new UserRepository(context);
                var actionRepository = new ActionRepository(context);

                var user = userRepository.Find(id);
                if (user == null)
                    RedirectToAction("Users");

                var actions = actionRepository.GetAll().OrderBy(x => x.Name);
                model.AvailableActions = mapper.Map<ICollection<AppActionViewModel>>(actions);
                model.User = mapper.Map<AppUserViewModel>(user);
                model.ResourcesList = PopulateResourceFromNewUserPermission(id);

                return View(model);
            }
        }
        [HttpPost]
        public ActionResult CreateUserPermission(NewAppUserPermissionViewModel model)
        {

            using (var context = new AppSecurityContext())
            {
                var actionRepository = new ActionRepository(context);
                var permissionRepository = new PermissionRepository(context);
                var userPermissionRepository = new UserPermissionRepository(context);
                var userRepository = new UserRepository(context);

                try
                {
                    var actions = actionRepository.Query(x => model.SelectedActions.Contains(x.Key)).Select(x => x.Key);
                    var permission = permissionRepository
                        .GetAll()
                        .Where(x => x.ResourceKey == model.SelectedResource)
                        .Where(x => actions.Contains(x.ActionKey));

                    userPermissionRepository.AddPermissionsByUserId(model.User.Id, permission.Select(x => x.Id).ToArray());
                    context.SaveChanges();
                    return RedirectToAction("UserPermission", new { id = model.User.Id });
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    var actions = actionRepository.GetAll().OrderBy(x => x.Name);
                    var user = userRepository.Find(model.User.Id);
                    model.User = mapper.Map<AppUserViewModel>(user);
                    model.AvailableActions = mapper.Map<ICollection<AppActionViewModel>>(actions);
                    model.ResourcesList = PopulateResourceFromNewUserPermission(model.SelectedResource);
                    return View(model);
                }
            }
        }

        public ActionResult EditUserPermission(int? id)
        {

            if (id == null)
                return RedirectToAction("DetailsUserPermission");

            var model = new EditAppUserPermissionViewModel();

            using (var context = new AppSecurityContext())
            {
                var actionRepository = new ActionRepository(context);
                var resourceRepository = new ResourceRepository(context);
                var userPermissionRository = new UserPermissionRepository(context);
                var userRepository = new UserRepository(context);

                var userPermision = userPermissionRository.GetAll().Where(x => x.Id == id).FirstOrDefault();
                var resource = resourceRepository.Find(userPermision.Permission.ResourceKey);

                var userPermissions = userPermissionRository.GetAll()
                    .Where(x => x.UserId == userPermision.UserId)
                    .Where(x => x.Permission.ResourceKey == resource.Key)
                    .ToList();


                var actionKeys = userPermissions.Select(x => x.Permission.ActionKey).ToArray();
                var actions = actionRepository.GetAll();

                model.AvailableActions = mapper.Map<ICollection<AppActionViewModel>>(actions);
                model.SelectedActions = actionKeys;
                model.ResourceKey = resource.Key;
                model.ResourceName = resource.Name;
                model.User = mapper.Map<AppUserViewModel>(userRepository.Find(userPermision.UserId));

                return View(model);
            }
        }
        [HttpPost]
        public ActionResult EditUserPermission(int id, EditAppUserPermissionViewModel model)
        { //Clave del resource

            using (var context = new AppSecurityContext())
            {
                var actionRepository = new ActionRepository(context);
                var resourceRepository = new ResourceRepository(context);
                var permissionRepository = new PermissionRepository(context);
                var userPermissionRository = new UserPermissionRepository(context);
                try
                {

                    var userPermision = userPermissionRository.GetAll().Where(x => x.Id == id).FirstOrDefault();

                    var actions = actionRepository.GetAll();
                    var resource = resourceRepository.Find(userPermision.Permission.ResourceKey);

                    var permissions = userPermissionRository.GetAll()
                        .Where(x => x.Permission.ResourceKey == resource.Key)
                        .Where(x => x.UserId == userPermision.UserId);

                    var actionKeys = permissions.Select(x => x.Permission.ActionKey).ToArray();
                    model.AvailableActions = mapper.Map<ICollection<AppActionViewModel>>(actions);

                    //Se eliminan los permisos anteriores
                    foreach (var perm in permissions)
                    {
                        userPermissionRository.Delete(perm);
                    }

                    if (model.SelectedActions != null)
                    {
                        //Se agregan los nuevos
                        var permissionForInsert = permissionRepository
                            .GetAll()
                            .Where(x => x.ResourceKey == resource.Key)
                            .Where(x => model.SelectedActions.Contains(x.ActionKey));

                        foreach (var p in permissionForInsert)
                        {
                            var permission = new AppUserPermission();
                            permission.UserId = userPermision.UserId;
                            permission.PermissionId = p.Id;
                            userPermissionRository.Insert(permission);
                        }
                        model.SelectedActions = actionKeys;
                        model.ResourceKey = resource.Key;
                        model.ResourceName = resource.Name;
                    }
                    context.SaveChanges();
                    return RedirectToAction("UserPermission", new { id = model.User.Id });
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    if (ex.InnerException != null)
                    {
                        ViewBag.Error += ex.InnerException.Message;
                    }
                    return View(model);
                }
            }

        }
        #endregion

        #region Roles
        public ActionResult Roles()
        {
            var context = new AppSecurityContext();
            var rolRepository = new RoleRepository(context);
            var roles = rolRepository.GetAll();
            var models = mapper.Map<IEnumerable<AppRoleViewModel>>(roles);
            return View(models);
        }

        public ActionResult CreateRole()
        {
            var context = new AppSecurityContext();
            var model = new NewAppRoleViewModel();
            var permissionRepository = new PermissionRepository(context);
            var permissions = permissionRepository.GetAll();
            model.AvailablePermissions = mapper.Map<ICollection<AppPermissionViewModel>>(permissions);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateRole(NewAppRoleViewModel model)
        {

            var context = new AppSecurityContext();
            var rolRepository = new RoleRepository(context);
            var permissionRepository = new PermissionRepository(context);
            var rolePermissionRepository = new RolePermissionRepository(context);

            if (ModelState.IsValid)
            {

                var role = mapper.Map<AppRole>(model);
                role.Id = Guid.NewGuid().ToString();
                rolRepository.Add(role);
                if (model.SelectedPermissions == null)
                    model.SelectedPermissions = new int[0];

                foreach (var permissionId in model.SelectedPermissions)
                {
                    rolePermissionRepository.Add(new AppRolePermission { PermissionId = permissionId, RoleId = role.Id });
                }
                context.SaveChanges();
                return RedirectToAction("Roles", "SecuritySettings");
            }

            var permissions = permissionRepository.GetAll();
            model.AvailablePermissions = mapper.Map<ICollection<AppPermissionViewModel>>(permissions);
            return View(model);
        }

        public ActionResult EditRole(string id)
        {
            var context = new AppSecurityContext();
            var rolePermissionRepository = new RolePermissionRepository(context);
            var permissionRepository = new PermissionRepository(context);

            var role = context.Roles.Find(id);
            var permissionsResult = rolePermissionRepository.GetPermissionByRoleId(id);
            var permissions = permissionRepository.GetAll();
            var model = mapper.Map<EditAppRoleViewModel>(role);

            if (permissionsResult.Count() > 0)
                model.SelectedPermissions = permissionsResult.Select(x => x.Id).ToArray();

            model.AvailablePermissions = mapper.Map<ICollection<AppPermissionViewModel>>(permissions);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditRole(string id, EditAppRoleViewModel model)
        {
            var context = new AppSecurityContext();
            var rolePermissionRepository = new RolePermissionRepository(context);
            var permissionRepository = new PermissionRepository(context);
            var roleRepostory = new RoleRepository(context);

            if (ModelState.IsValid)
            {

                var role = mapper.Map<AppRole>(model);
                roleRepostory.UpdateRoleWithPermissions(role, model.SelectedPermissions);
                context.SaveChanges();
                return RedirectToAction("Roles", "SecuritySettings");
            }
            var permissions = roleRepostory.GetAll();
            if (permissions.Count() > 0)
                model.AvailablePermissions = mapper.Map<ICollection<AppPermissionViewModel>>(permissions);

            return View(model);

        }


        public ActionResult DetailsRole(string id)
        {

            var context = new AppSecurityContext();
            var roleRepository = new RoleRepository(context);
            var rolePermissionRepository = new RolePermissionRepository(context);

            var role = roleRepository.Find(id);

            var permissionsResult = rolePermissionRepository.GetPermissionsByRoleIncludingActionResource(id);

            var model = mapper.Map<DetailsAppRoleViewModel>(role);
            model.Permissions = new List<AppPermissionViewModel>();

            foreach (var result in permissionsResult)
            {

                model.Permissions.Add(new AppPermissionViewModel { ActionName = result.Action.Name, ResourceName = result.Resource.Name });
            }

            return View(model);
        }

        #endregion

        #region Menús

        public ActionResult Menus()
        {
            var context = new AppSecurityContext();
            var menuRepository = new MenuRepository(context);
            var menus = menuRepository.GetAll();
            var model = mapper.Map<ICollection<MenuViewModel>>(menus);
            return View(model);
        }

        public ActionResult CreateMenu()
        {
            var context = new AppSecurityContext();
            return View();
        }

        [HttpPost]
        public ActionResult CreateMenu(NewMenuViewModel model)
        {
            var context = new AppSecurityContext();
            var menuRepository = new MenuRepository(context);
            if (ModelState.IsValid)
            {

                var menu = mapper.Map<AppMenu>(model);
                menuRepository.Add(menu);
                context.SaveChanges();
                return RedirectToAction("Menus");
            }
            return View();
        }

        public ActionResult EditMenu(string id)
        {
            var context = new AppSecurityContext();
            var menuRepository = new MenuRepository(context);
            var menu = menuRepository.Find(id);
            var model = mapper.Map<EditMenuViewModel>(menu);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditMenu(string id, EditMenuViewModel model)
        {
            var context = new AppSecurityContext();
            var menuRepository = new MenuRepository(context);
            try
            {
                if (ModelState.IsValid)
                {
                    var menu = mapper.Map<AppMenu>(model);
                    menuRepository.Update(menu);
                    context.SaveChanges();
                    return RedirectToAction("Menus");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(model);
        }

        public ActionResult MenuItems(string id)
        {
            var context = new AppSecurityContext();
            var menuRepository = new MenuRepository(context);
            var menuItemRepository = new MenuItemRepository(context);
            var menu = menuRepository.Find(id);
            var items = menuItemRepository.GetItemsByMenuKey(id);
            var model = new MenuItemListViewModel();
            model.MenuItems = mapper.Map<ICollection<MenuItemViewModel>>(items);
            model.MenuKey = menu.Key;
            model.MenuName = menu.Name;
            return View(model);
        }

        public ActionResult CreateMenuItem(string id)
        {
            var context = new AppSecurityContext();
            var menuRepository = new MenuRepository(context);
            var menu = menuRepository.Find(id);
            var model = new NewMenuItemViewModel();
            model.MenuName = menu.Name;
            model.MenuKey = menu.Key;
            model.AvailablePermissions = PopulatePermissions(model.PermissionId);
            model.AvailableMenuItems = PopulateMenuItems(model.ParentId);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateMenuItem(string id, NewMenuItemViewModel model)
        {

            var context = new AppSecurityContext();
            var menuItemRepository = new MenuItemRepository(context);
            try
            {
                var menuItem = mapper.Map<AppMenuItem>(model);
                menuItemRepository.Add(menuItem);
                context.SaveChanges();
                return RedirectToAction("menuItems", new { id = model.MenuKey });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            model.AvailablePermissions = PopulatePermissions(model.PermissionId);
            model.AvailableMenuItems = PopulateMenuItems(model.ParentId);
            return View(model);
        }


        [HttpGet]
        public ActionResult EditMenuItem(int id) //Id del item
        {
            var context = new AppSecurityContext();
            var menuRepository = new MenuRepository(context);
            var menuItemRepository = new MenuItemRepository(context);

            var menuItem = menuItemRepository.Find(id);
            var model = mapper.Map<EditMenuItemViewModel>(menuItem);
            model.MenuKey = menuItem.AppMenuKey;
            model.MenuName = menuItem.AppMenu.Name;
            model.AvailablePermissions = PopulatePermissions(model.PermissionId);
            model.AvailableMenuItems = PopulateMenuItems(model.ParentId, id);


            return View(model);
        }


        [HttpPost]
        public ActionResult EditMenuItem(int id, EditMenuItemViewModel model)
        {
            var context = new AppSecurityContext();
            var menuRepository = new MenuRepository(context);
            var menuItemRepository = new MenuItemRepository(context);
            try
            {
                if (ModelState.IsValid)
                {
                    var menuItem = mapper.Map<AppMenuItem>(model);
                    menuItemRepository.Update(menuItem);
                    context.SaveChanges();
                    return RedirectToAction("menuItems", new { id = model.MenuKey });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }



            model.AvailablePermissions = PopulatePermissions(model.PermissionId);
            model.AvailableMenuItems = PopulateMenuItems(model.ParentId);

            var itemSelf = model.AvailableMenuItems.Where(x => x.Value == id.ToString());
            if (itemSelf != null && itemSelf.Count() > 0)
            {
                var itemForRemove = itemSelf.SingleOrDefault();
                model.AvailablePermissions.ToList().Remove(itemForRemove);
            }
            return View(model);
        }


        public SelectList PopulateResources(object selectedItem = null)
        {

            using (var context = new AppSecurityContext())
            {
                var repository = new ResourceRepository(context);
                var resources = repository.GetAll().OrderBy(x => x.Name).ToList();
                resources.Insert(0, new AppResource { Key = null, Name = "Seleccione" });
                return new SelectList(resources, "Key", "Name", selectedItem);
            }
        }

        public SelectList PopulateResourcesForNewPermission(object selectedItem = null)
        {
            using (var context = new AppSecurityContext())
            {
                var repository = new ResourceRepository(context);
                var permissionRepository = new PermissionRepository(context);
                var resourcesId = permissionRepository.GetAll().Select(x => x.ResourceKey);
                var resources = repository.Query(x => !resourcesId.Contains(x.Key)).ToList();
                resources.Insert(0, new AppResource { Key = null, Name = "Seleccione" });
                return new SelectList(resources, "Key", "Name", selectedItem);
            }
        }


        public SelectList PopulatePermissions(object selectedItem = null)
        {
            var context = new AppSecurityContext();
            var repository = new PermissionRepository(context);
            var permissions = repository.GetAll().OrderBy(x => x.ResourceKey);

            var permissionList = new List<SelectListItem>();
            permissionList.Add(new SelectListItem { Value = null, Text = "Sin permiso" });
            foreach (var perm in permissions)
            {
                var permDesc = $"{perm.Resource.Name} - {perm.Action.Name}";
                permissionList.Add(new SelectListItem { Value = perm.Id.ToString(), Text = permDesc });
            }
            return new SelectList(permissionList, "Value", "Text", selectedItem);
        }

        public SelectList PopulateMenuItems(object selectedItem = null, int? excludedId = null)
        {
            var context = new AppSecurityContext();
            var repository = new MenuItemRepository(context);
            var items = repository.GetAll().OrderBy(x => x.Name).ToList();

            if (excludedId != null)
            {
                var itemExcluded = items.SingleOrDefault(x => x.Id == excludedId);
                items.Remove(itemExcluded);
            }
            items.Insert(0, new AppMenuItem { Id = null, Name = "Sin padre" });

            return new SelectList(items, "Id", "Name", selectedItem);
        }

        public SelectList PopulateResourceFromNewUserPermission(string userId, object selectedItem = null)
        {

            using (var context = new AppSecurityContext())
            {
                var resourcesRepository = new ResourceRepository(context);
                var userPermissionRository = new UserPermissionRepository(context);

                var permissionSelect = userPermissionRository.GetPermissionByUserId(userId).Select(p => p.ResourceKey).Distinct();
                var resourcesQuery = resourcesRepository.GetAll();
                var resources = resourcesQuery.Where(x => !permissionSelect.Contains(x.Key)).ToList();
                resources.Insert(0, new AppResource { Key = null, Name = "Seleccione" });
                return new SelectList(resources, "Key", "Name", selectedItem);
            }

        }

        #endregion

    }
}