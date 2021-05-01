using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
        //GERAIS
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

         public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

         public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAssyn()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        
        //EVENTO
        public async Task<Evento[]> GetAllEventoAssync(bool includePalestrantes = false )
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSociais);

            if(includePalestrantes)
            {
                query = query 
                    .Include( pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                          .OrderByDescending(c => c.DataEvento);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventoAssyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSociais);

            if(includePalestrantes)
            {
                query = query 
                    .Include( pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                         .OrderByDescending(c => c.DataEvento)
                         .Where(c => c.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }
               
        public async Task<Evento> GetEventoAssyncById(int Eventoid, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSociais);

            if(includePalestrantes)
            {
                query = query 
                    .Include( pe => pe.PalestrantesEventos)
                    .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                         .OrderByDescending(c => c.DataEvento)
                         .Where(c => c.Id == Eventoid);

            return await query.FirstOrDefaultAsync();
        }

        //PALESTRANTE
        public async Task<Palestrante> GetPalestranteAssync(int Palestranteid, bool includeEvento = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedeSociais);

            if(includeEvento)
            {
                query = query 
                    .Include( e => e.PalestrantesEventos)
                    .ThenInclude(ev => ev.Evento);
            }

            query = query.OrderBy(p => p.Nome)
                .Where(p => p.Id == Palestranteid);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Palestrante[]> GetAllPalestranteAssyncName(string name, bool includeEvento = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c => c.RedeSociais);

            if(includeEvento)
            {
                query = query 
                    .Include( e => e.PalestrantesEventos)
                    .ThenInclude(ev => ev.Evento);
            }

            query = query.AsNoTracking()
                         .Where(p => p.Nome.ToLower().Contains(name.ToLower()));

            return await query.ToArrayAsync();
        }

        Task<Evento[]> IProAgilRepository.GetEventoAssyncById(int Eventoid, bool includePalestrantes)
        {
            throw new System.NotImplementedException();
        }

        Task<Palestrante> IProAgilRepository.GetPalestranteAssyncById(int Palestranteid, bool includeEvento)
        {
            throw new System.NotImplementedException();
        }
    }
}