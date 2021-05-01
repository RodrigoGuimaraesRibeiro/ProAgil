using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        ///GERAL
         void Add<T> (T entity) where T: class;
         void Update<T> (T entity) where T: class;
         void Delete<T> (T entity) where T: class;
         Task<bool> SaveChangesAssyn();

         ///EVENTOS
         Task<Evento[]> GetAllEventoAssyncByTema(string tema, bool includePalestrantes);
         Task<Evento[]> GetAllEventoAssync(bool includePalestrantes);
         Task<Evento[]> GetEventoAssyncById(int Eventoid, bool includePalestrantes);

         ///PALESTRANTE
         Task<Palestrante[]> GetAllPalestranteAssyncName(string name, bool includeEvento);
         Task<Palestrante> GetPalestranteAssyncById(int Palestranteid, bool includeEvento);
        
    }
}