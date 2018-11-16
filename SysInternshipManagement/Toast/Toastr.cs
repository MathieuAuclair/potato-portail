using System;
using System.Collections.Generic;
using ApplicationPlanCadre.Toast;

namespace SysInternshipManagement.Toast
{
    [Serializable]
    public class Toastr
    {
        public bool AfficherRecentOnTop { get; set; }
        public bool AfficherBoutonFermer { get; set; }
        public List<ToastMessage> ToastMessages { get; set; }

        public ToastMessage AddToastMessage(string title, string message, ToastType toastType)
        {
            var toast = new ToastMessage()
            {
                Title = title,
                Message = message,
                ToastType = toastType
            };

            ToastMessages.Add(toast);
            return toast;
        }

        public Toastr()
        {
            ToastMessages = new List<ToastMessage>();
            AfficherRecentOnTop = false;
            AfficherBoutonFermer = false;
        }
    }
}