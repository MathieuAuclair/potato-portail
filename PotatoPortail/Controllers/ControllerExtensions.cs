using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationPlanCadre.Toast;
using PotatoPortail.Toast;

namespace ApplicationPlanCadre.Controllers
{
    public static class ControllerExtensions
    {
        public static ToastMessage AddToastMessage(this Controller controller, string title, string message, ToastType toastType = ToastType.Info, bool isSticky = false)
        {
            Toastr toastr = controller.TempData["Toastr"] as Toastr;
            toastr = toastr ?? new Toastr();

            var toastMessage = toastr.AddToastMessage(title, message, toastType, isSticky);
            controller.TempData["Toastr"] = toastr;
            return toastMessage;
        }
    }
}
