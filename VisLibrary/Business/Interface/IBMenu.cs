using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisLibrary.Models;
using VisLibrary.Models.ViewModel;

namespace VisLibrary.Business.Interface
{
    public interface IBMenu
    {
        Task<List<MMenu>> GetList();
        Task<MMenu?> Get(int pk);
        Task<MMenu?> GetByController(string controller);
        Task<List<MMenu>> GetEnableList();
        List<MNode1Menu> GetNode1NameList(List<MMenu> list);
        string GetNode1Active(List<MMenu> list, MNode1Menu node1, string controller);
        List<MMenu> GetNode2List(List<MMenu> list, string node1Name);
        string GetNode2Active(MMenu MMenu, string controller);
        string GetTitle(List<MMenu> list, string controller);
        Task Set(MMenu obj);
        Task Delete(int pk);
    }
}