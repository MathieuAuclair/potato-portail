using System.Collections.Generic;
using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class PosteController
    {
        private readonly DatabaseContext _bd = new DatabaseContext();
        
        public IEnumerable<Poste> RecolterTousLesPostes()
        {
              return (
                from entity
                    in _bd.poste
                select entity
            );
        }
    }
}