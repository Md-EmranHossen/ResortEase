using ResortEase.Application.Common.Interfaces;
using ResortEase.Domain.Entities;
using ResortEase.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResortEase.Infrastructure.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(VillaNumber entity)
        {
            _db.Update(entity);
        }
    }
}
