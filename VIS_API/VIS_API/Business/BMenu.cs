using VIS_API.Models.ViewModel;
using System.Runtime.InteropServices;
using VIS_API.Business.Interface;
using VIS_API.Models;
using VIS_API.Repositories;
using VIS_API.Repositories.Interface;
using VIS_API.Service;
using VIS_API.Utilities;
namespace VIS_API.Business
{
    public class BMenu : IBMenu
    {
        private const string CACHE_KEY = "BMenu.GetList";

        private readonly IRMenu _Rmenu;

        public BMenu(IRMenu Rmenu)
        {
            _Rmenu = Rmenu;
        }

        public async Task<MMenu?> Get(int pk)
        {
            return (await GetList()).Find(x => x.pk == pk);
        }

        public async Task<MMenu?> GetByController(string controller)
        {
            return (await GetList()).Find(x => x.controller == controller);
        }

        public async Task<List<MMenu>> GetList()
        {
                Cache.Set(CACHE_KEY, await _Rmenu.GetAll());

                return Cache.Get<List<MMenu>>(CACHE_KEY) ?? new List<MMenu>();

            //var newConnectionInfo = new DbConnectionInfo { Charset= "utf8", Database = "new_dekviserp", Server= "172.23.9.101", DbType = "MySql", id = "dek", Password = "54886961" ,Port= 3306 };

            //if (Cache.Get<List<MMenu>>(CACHE_KEY) == null)
            
        }


        // ���A�� V �� MMenu
        public async Task<List<MMenu>> GetEnableList()
        {
            return (await GetList()).FindAll(x => x.status == "V");
        }
        public async Task<List<MMenu>> NewGetEnableList()
        {
            return (await GetList()).FindAll(x => x.status == "V");
        }

        // ���o node1 �� list
        public List<MNode1Menu> GetNode1NameList(List<MMenu> _list)
        {
            return _list.Select(x => new MNode1Menu
            {
                node1_name = x.node1,
                node1_icon = x.icon
            })
            .GroupBy(x => x.node1_name)
            .Select(x => x.First())
            .ToList();
        }

        // �ˬd���e��controller �� node1 �O�_�� active
        public string GetNode1Active(List<MMenu> _list, MNode1Menu node1, string controller)
        {
            return _list.Exists(
                x => x.node1 == node1.node1_name && x.controller == controller)
                ? "active" : "";
        }

        // ���o���e node1 ���U�� node2 �� list
        public List<MMenu> GetNode2List(List<MMenu> _list, string node1_name)
        {
            return _list.FindAll(x => x.node1 == node1_name);
        }

        // �ˬd���e��controller �� node2 �O�_�� current-page
        public string GetNode2Active(MMenu MMenu, string controller)
        {
            return MMenu.controller == controller ? "current-page" : "";
        }

        // ���o���e�� node1/node2
        public string GetTitle(List<MMenu> _list, string controller)
        {
            var MMenu = _list.Find(x => x.controller == controller);
            return MMenu == null ? "" : $"{MMenu.node1} / {MMenu.node2}";
        }

        public async Task Set(MMenu obj)
        {
            if (obj.pk == 0)
                obj.pk = await _Rmenu.Insert(obj);
            else
                _ = await _Rmenu.Update(obj);

            Cache.Remove(CACHE_KEY);
        }

        public async Task Delete(int pk)
        {
            _ = await _Rmenu.Delete(pk);

            Cache.Remove(CACHE_KEY);
        }

    }
}