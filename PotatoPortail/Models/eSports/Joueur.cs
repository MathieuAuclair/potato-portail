using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PotatoPortail.Migrations;

namespace PotatoPortail.Models.eSports
{
    public partial class Joueur
    {
        private readonly BdPortail _db = new BdPortail();

        public Joueur()
        {
            Equipe = new HashSet<Equipe>();
            Item = new HashSet<Item>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Pseudo")]
        public string PseudoJoueur { get; set; }

        public string IdMembreESports { get; set; }

        public virtual MembreESports MembreESports { get; set; }

        public virtual Profil Profil { get; set; }

        public virtual ICollection<Equipe> Equipe { get; set; }

        public virtual ICollection<Item> Item { get; set; }

        public virtual ICollection<HistoriqueRang> HistoriquesRang { get; set; }

        public Equipe EquipeMonojoueur
        {
            get
            {
                var equipeMonojoueurs = from e in _db.Equipe
                                        join j in _db.Jeu on e.IdJeu equals j.Id
                                        join p in _db.Profil on e.IdJeu equals p.IdJeu
                                        join membreESports in _db.MembreESports on p.IdMembreESports equals membreESports.Id
                                        join joueur in _db.Joueur on membreESports.Id equals joueur.IdMembreESports
                                        where e.EstMonoJoueur && (e.NomEquipe == MembreESports.NomComplet + "_" + e.Jeu.Abreviation + "_" + joueur.Profil.IdMembreESports) && (e.Jeu.NomJeu == p.Jeu.NomJeu) && (joueur.Id == Id)
                                        select e;

                int idJeuJoueur = Profil.IdJeu;

                foreach (Equipe equipeMonojoueur in equipeMonojoueurs)
                {
                    if (equipeMonojoueur.IdJeu == idJeuJoueur)
                    {
                        return equipeMonojoueur;
                    }
                }

                return null;
            }
        }

        public string JeuEquipeMonojoueur => EquipeMonojoueur.Jeu.NomJeu;

        public int NombreJeuJoueur
        {
            get
            {
                var nomEquipeMonojoueur = EquipeMonojoueur.NomEquipe;
                var index = nomEquipeMonojoueur.IndexOf("_", StringComparison.Ordinal);
                var indexIdMembreESports = nomEquipeMonojoueur.IndexOf("_", nomEquipeMonojoueur.IndexOf("_", StringComparison.Ordinal) + 2, StringComparison.Ordinal);
                var nomCompletEtu = nomEquipeMonojoueur.Substring(0, index);
                var idEtuEquipeMonojoueur = nomEquipeMonojoueur.Substring(indexIdMembreESports);

                var jeuJoueur = from e in _db.Equipe
                                 where e.EstMonoJoueur && (e.NomEquipe.StartsWith(nomCompletEtu)) && (("_" + IdMembreESports) == idEtuEquipeMonojoueur)
                                 select e;

                return jeuJoueur.Count();
            }
        }
    }
}