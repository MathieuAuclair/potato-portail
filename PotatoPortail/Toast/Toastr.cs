using System;
using System.Collections.Generic;

namespace PotatoPortail.Toast
{
    [Serializable]
    public class Toastr
    {
        public bool AfficherRecentOnTop { get; set; }
        public bool AfficherBoutonFermer { get; set; }
        public List<ToastMessage> ToastMessages { get; set; }

        public ToastMessage AddToastMessage(string title, string message, ToastType toastType, bool isSticky)
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