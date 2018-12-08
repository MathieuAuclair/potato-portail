using System.Web.Mvc;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    public static class ControllerExtensions
    {
        public static ToastMessage AddToastMessage(this Controller controller, string title, string message, ToastType toastType = ToastType.Info, bool isSticky = false)
        {
            var toastr = controller.TempData["Toastr"] as Toastr;
            toastr = toastr ?? new Toastr();

            var toastMessage = toastr.AddToastMessage(title, message, toastType, isSticky);
            controller.TempData["Toastr"] = toastr;
            return toastMessage;
        }
    }
}
