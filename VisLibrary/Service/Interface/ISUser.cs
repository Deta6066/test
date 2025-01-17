using VisLibrary.Models;

namespace VisLibrary.Service.Interface
{
    public interface ISUser
    {
        Task<int> Delete(uint pk);
        Task<MUser?> Get(uint pk);
        Task<MUser?> GetByAcc(string acc,string pwd);
        Task<List<MUser>> GetList();
        Task<uint> Insert(MUser obj, bool autoIncrement = true);
        Task<int> Update(MUser obj);
    }
}