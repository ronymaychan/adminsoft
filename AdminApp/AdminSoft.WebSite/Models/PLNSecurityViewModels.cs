using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLNFramework.Security.ViewModels
{
    #region ViewModels
    public class AppUserViewModel
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }


    public class EditAppUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public ICollection<AppRoleViewModel> AvailableRoles { get; set; }
        public string[] SelectedRoles { get; set; }
    }

    public class DetailsAppUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public ICollection<AppRoleViewModel> AssignedRoles { get; set; }
        public IEnumerable<AppUserPermissionViewModel> UserPermissions { get; set; }
    }

    public class AppRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class NewAppRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<AppPermissionViewModel> AvailablePermissions { get; set; }
        public int[] SelectedPermissions { get; set; }
    }

    public class EditAppRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<AppPermissionViewModel> AvailablePermissions { get; set; }
        public int[] SelectedPermissions { get; set; }
    }

    public class DetailsAppRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<AppPermissionViewModel> Permissions { get; set; }
    }

    public class AppPermissionViewModel
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string ActionKey { get; set; }
        public string ResourceName { get; set; }
        public string ResourceKey { get; set; }
    }

    public class NewAppPermissionViewModel {
        public SelectList ResourcesList { get; set; }
        public ICollection<AppActionViewModel> AvailableActions { get; set; }
        [Required]
        public string SelectedResource { get; set; }
        [Required]
        public string[] SelectedActions { get; set; }
    }

    public class EditAppPermissionViewModel
    {
        public string ResourceKey { get; set; }
        public string ResourceName { get; set; }

        [Required]
        public ICollection<AppActionViewModel> AvailableActions { get; set; }

        [Required]
        public string[] SelectedActions { get; set; }
    }


    public class AppActionViewModel
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class AppResourceViewModel
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class NewAppUserPermissionViewModel
    {
        public AppUserViewModel User { get; set; }
        public SelectList ResourcesList { get; set; }
        public ICollection<AppActionViewModel> AvailableActions { get; set; }
        [Required]
        public string SelectedResource { get; set; }
        [Required]
        public string[] SelectedActions { get; set; }
    }

    public class EditAppUserPermissionViewModel {
        public int? Id { get; set; }
        public AppUserViewModel User { get; set; }  
        [Required]
        public ICollection<AppActionViewModel> AvailableActions { get; set; }
        [Required]
        public string[] SelectedActions { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceName { get; set; }
    }

    public class AppUserPermissionViewModel {
        public int? Id { get; set; }
        public string UserId { get; set; }
        public string ActionName { get; set; }
        public string ActionKey { get; set; }
        public string ResourceName { get; set; }
        public string ResourceKey { get; set; }
    }

    public class ListAppUserPermissionViewModel {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<AppUserPermissionViewModel> UserPermissions { get; set; }
    }


    #endregion

    #region Menus
    public class MenuViewModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public AppRoleViewModel Role { get; set; }
    }

    public class NewMenuViewModel
    {
        [Required]
        [MaxLength(25)]
        public string Key { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public bool? IsActive { get; set; }
    }

    public class EditMenuViewModel
    {
        [Required]
        [MaxLength(25)]
        public string Key { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public bool? IsActive { get; set; }
    }

    public class MenuItemListViewModel
    {
        public string MenuKey { get; set; }
        public string MenuName { get; set; }
        public ICollection<MenuItemViewModel> MenuItems { get; set; }
    }

    public class MenuItemViewModel
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string PathToResource { get; set; }
        public int? ParentId { get; set; }
        public MenuItemViewModel Parent { get; set; }
    }

    public class NewMenuItemViewModel
    {
        [MaxLength(25)]
        public string MenuKey { get; set; }
        [MaxLength(50)]
        public string MenuName { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Order { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PathToResource { get; set; }
        public SelectList AvailablePermissions { get; set; }
        public int? PermissionId { get; set; }
        public SelectList AvailableMenuItems { get; set; }
        public int? ParentId { get; set; }
    }


    public class EditMenuItemViewModel
    {
        public int Id { get; set; }
        [MaxLength(25)]
        public string MenuKey { get; set; }
        [MaxLength(50)]
        public string MenuName { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Order { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PathToResource { get; set; }
        public SelectList AvailablePermissions { get; set; }
        public int? PermissionId { get; set; }
        public SelectList AvailableMenuItems { get; set; }
        public int? ParentId { get; set; }
    }
    #endregion

    #region Login
    public class AppLoginViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordar cuenta?")]
        public bool RememberMe { get; set; }
    }
    #endregion
}