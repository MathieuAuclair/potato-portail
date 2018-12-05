using System.Net.Mail;
using PotatoPortail.Models;

namespace PotatoPortail.Helpers
{
    public class MailHelper
    {
        readonly SmtpClient _client;

        public MailHelper()
        {
            _client = new SmtpClient
            {
                Port = 587,
                Host = "mail.dicj.info",
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("equipe1@dicj.info", "equipe1")
            };
        }

        public bool SendActivationMail(ApplicationUser user, string password)
        {
            return SendMail(user, "Bienvenue sur Portail!", BuildActivationMail(user, password));
        }

        public bool SendEditMail(ApplicationUser user, string password)
        {
            return SendMail(user, "Modification de votre compte sur portail", BuildEditMail(user, password));
        }

        private bool SendMail(ApplicationUser user, string subject, string body)
        {
            MailMessage message =
                new MailMessage("portaildonotreply@dicj.info", user.Email, subject, body) {IsBodyHtml = true};
            try
            {
                _client.Send(message);
                return true;
            }
            catch (SmtpException)
            {
                return false;
            }
        }

        private string BuildActivationMail(ApplicationUser user, string password)
        {
            return "<p> Bonjour " + user.prenom + ",</p>" +
                   "<p>Un compte à été crée pour vous, rendez vous sur Portail afin de vous connectez avec les informations ci dessous.</p>" +
                   "<p>Vous allez pouvoir changer votre mot de passe une fois connecté.</p>" +
                   "<p>Cordialement,</p>" +
                   "<p>L'équipe de Portail</p>" +
                   "<p><b> Courriel: </b>" + user.Email + "<br>" +
                   "<b> Mot de passe: </b>" + password + "</p>" +
                   "<p><a href='http://deptinfo420/Projet2017/Equipe1/test'>Rendez-vous sur portail</a></p>";
        }

        private string BuildEditMail(ApplicationUser user, string password)
        {
            return "<p> Bonjour " + user.prenom + ",</p>" +
                   "<p>Des informations ont été modifié sur votre compte et il a été nécessaire de réinitialiser votre mot de passe.</p>" +
                   "<p>Vous devez vous connecter avec les informations ci dessous</p>" +
                   "<p>Vous allez pouvoir changer votre mot de passe une fois connecté.</p>" +
                   "<p>Cordialement,</p>" +
                   "<p>L'équipe de Portail</p>" +
                   "<p><b> Courriel: </b>" + user.Email + "<br>" +
                   "<b> Mot de passe: </b>" + password + "</p>" +
                   "<p><a href='http://deptinfo420/Projet2017/Equipe1/test'>Rendez-vous sur portail</a></p>";
        }
    }
}