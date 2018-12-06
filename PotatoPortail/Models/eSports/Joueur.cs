namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Joueur
    {
        private BDPortail db = new BDPortail();

        public Joueur()
        {
            Equipe = new HashSet<Equipe>();
            Item = new HashSet<Item>();
        }

        public int Id { get; set; }

        [Required]
        public string PseudoJoueur { get; set; }

        public int IdEtudiant { get; set; }

        public int IdProfil { get; set; }

        [Required]
        [StringLength(128)]
        public string IdMembreESports { get; set; }

        public virtual ICollection<HistoriqueRang> HistoriquesRang { get; set; }

        public virtual MembreESports MembreESports { get; set; }

        public virtual Profil Profil { get; set; }

        public virtual ICollection<Equipe> Equipe { get; set; }

        public virtual ICollection<Item> Item { get; set; }

        public Equipe equipeMonojoueur
        {
            get
            {
                var equipeMonojoueurs = from e in db.Equipe
                                        join j in db.Jeu on e.IdJeu equals j.Id
                                        join p in db.Profil on e.IdJeu equals p.IdJeu
                                        join etu in db.MembreESports on p.IdMembreESports equals etu.id.ToString()
                                        join joueur in db.Joueur on etu.id equals joueur.MembreESportsId
                                        where (e.estMonojoueur == true) && (e.nomEquipe == MembreESports.nomComplet + "_" + e.Jeu.abreviation + "_" + joueur.Profil.MembreESportsId) && (e.Jeu.nomJeu == p.Jeu.nomJeu) && (joueur.id == id)
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

        public string jeuEquipeMonojoueur
        {
            get
            {
                return equipeMonojoueur.Jeu.NomJeu;
            }
        }

        public int nombreJeuxJoueur
        {
            get
            {
                string nomEquipeMonojoueur = equipeMonojoueur.NomEquipe;
                int index = nomEquipeMonojoueur.IndexOf("_");
                int indexIdMembreESports = nomEquipeMonojoueur.IndexOf("_", nomEquipeMonojoueur.IndexOf("_") + 2);
                string nomCompletEtu = nomEquipeMonojoueur.Substring(0, index);
                string idEtuEquipeMonojoueur = nomEquipeMonojoueur.Substring(indexIdMembreESports);

                var jeuxJoueur = from e in db.Equipe
                                 where (e.EstMonojoueur == true) && (e.NomEquipe.StartsWith(nomCompletEtu)) && (("_" + IdMembreESports) == idEtuEquipeMonojoueur)
                                 select e;

                return jeuxJoueur.Count();
            }
        }
    }
}
