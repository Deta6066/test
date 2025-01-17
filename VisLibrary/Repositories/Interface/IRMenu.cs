using VisLibrary.Models;

namespace VisLibrary.Repositories.Interface
{
    public interface IRMenu : IGenericRepositoryBase<MMenu>
    {
        public Task<MMenu?> GetByPk(int? pk);
        public Task<int> Insert(MMenu obj, bool autoIncrement = true);
        public Task<int> Update(MMenu obj);
        public Task<int> Delete(int pk);
        public Task<List<MMenu>> GetAll();

    }
}
