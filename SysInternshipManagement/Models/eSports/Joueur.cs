using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ApplicationPlanCadre.Models;
using ApplicationPlanCadre.Models.eSports;
using SysInternshipManagement.Migrations;

namespace SysInternshipManagement.Models.eSports
{
    [Table("Joueurs")]
    public class Joueur
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        public Joueur()
        {
            Equipe = new HashSet<Equipe>();
            Item = new HashSet<Item>();
        }

        [Display(Name = "ID")] public int Id { get; set; }

        [Required] [Display(Name = "Pseudo")] public string PseudoJoueur { get; set; }

        [Display(Name = "ID Étudiant")] public int EtudiantId { get; set; }

        public virtual Etudiant Etudiant { get; set; }

        public virtual Profil Profil { get; set; }

        public virtual ICollection<Item> Item { get; set; }

        public virtual ICollection<Equipe> Equipe { get; set; }

        public virtual ICollection<HistoriqueRang> HistoriqueRang { get; set; }

        public Equipe EquipeMonojoueur
        {
            get
            {
                var equipeMonojoueurs = from equipe in _db.Equipes
                    join jeu in _db.Jeux on equipe.JeuId equals jeu.id
                    join profil in _db.Profils on equipe.JeuId equals profil.JeuId
                    join etudiant in _db.Etudiant on profil.EtudiantId equals etudiant.IdEtudiant
                    join joueur in _db.Joueurs on etudiant.IdEtudiant equals joueur.EtudiantId
                    where equipe.estMonojoueur &&
                          (equipe.nomEquipe == Etudiant.Prenom + Etudiant.Prenom + "_" + equipe.Jeu.abreviation + "_" +
                           joueur.Profil.EtudiantId) &&
                          (equipe.Jeu.nomJeu == profil.Jeu.nomJeu) &&
                          (joueur.Id == Id)
                    select equipe;

                int idJeuJoueur = Profil.JeuId;

                foreach (Equipe equipeMonojoueur in equipeMonojoueurs)
                {
                    if (equipeMonojoueur.JeuId == idJeuJoueur)
                    {
                        return equipeMonojoueur;
                    }
                }

                return null;
            }
        }

        public string JeuEquipeMonojoueur => EquipeMonojoueur.Jeu.nomJeu;

        public int NombreJeuxJoueur
        {
            get
            {
                string nomEquipeMonojoueur = EquipeMonojoueur.nomEquipe;
                int index = nomEquipeMonojoueur.IndexOf("_", StringComparison.Ordinal);
                string nomCompletEtu = nomEquipeMonojoueur.Substring(0, index);
                int idEtuEquipeMonojoueur =
                    Convert.ToInt32(nomEquipeMonojoueur.Substring(nomEquipeMonojoueur.Length - 1, 1));

                var jeuxJoueur = from e in _db.Equipes
                    where e.estMonojoueur && (e.nomEquipe.StartsWith(nomCompletEtu)) &&
                          (EtudiantId == idEtuEquipeMonojoueur)
                    select e;

                return jeuxJoueur.Count();
            }
        }
    }
}