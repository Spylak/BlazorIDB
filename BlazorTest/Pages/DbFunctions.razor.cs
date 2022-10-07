using BlazorIDB;
using BlazorTest.Database;
using BlazorTest.Entities;
using BlazorTest.Services;
using BlazorTest.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorTest.Pages
{
    public partial class DbFunctions
    {
        private string EntityId { get; set; }
        private Random Random = new Random();
        [Inject] private IndexedDb _indexedDb { get; set; }
        [Inject] private IGlobalService globalService { get; set; }
        private List<MyEntity> MyEntities = new List<MyEntity>();
        private HashSet<MyEntity> SelectedItems = new HashSet<MyEntity>();
        private MyEntity GetOneEntity { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (_indexedDb == null)
                return;
            await LoadData();
        }

        private async Task LoadData()
        {
            var result = await _indexedDb?.Entities?.GetAll();
            MyEntities = result?.Data?.ToList() ?? new List<MyEntity>();
        }

        private async Task Add()
        {
            var myEnt = new MyEntity()
            {
                Id = Guid.NewGuid().ToString(),
                StringProp = $"New Prop {Random.Next(0, 1000)}",
                IntProp = Random.Next(0, 1000),
                IntList = new List<int>(){
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000)
            },
                InnerProperty = new MyEntity.InnerClass()
                {
                    InnerString = $"Random string nubmer {Random.Next(0, 1000)}",
                    StringList = new List<string>(){
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}"
                    }
                }
            };

            await _indexedDb.Entities.Add(myEnt);
            await LoadData();
            StateHasChanged();
        }

        private async Task GetById(string id)
        {
            var getById = (await _indexedDb.Entities.GetById(id)).Data;
            if (getById is null)
                return;
            MyEntities = new List<MyEntity>();
            MyEntities.Add(getById);
            StateHasChanged();
        }
        private async Task AddRange()
        {
            await _indexedDb.Entities.AddRange(new List<MyEntity>()
        {
            new MyEntity()
        {
            Id = Guid.NewGuid().ToString(),
            StringProp = $"New Prop {Random.Next(0, 1000)}",
            IntProp = Random.Next(0,1000),
            IntList =  new List<int>(){
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000)
            },
            InnerProperty = new MyEntity.InnerClass(){
                    InnerString = $"Random string nubmer {Random.Next(0, 1000)}",
                    StringList = new List<string>(){
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}"
                    }
            }
        },
            new MyEntity()
        {
            Id = Guid.NewGuid().ToString(),
            StringProp = $"New Prop {Random.Next(0, 1000)}",
            IntProp = Random.Next(0,1000),
            IntList =  new List<int>(){
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000)
            },
            InnerProperty = new MyEntity.InnerClass(){
                    InnerString = $"Random string nubmer {Random.Next(0, 1000)}",
                    StringList = new List<string>(){
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}"
                    }
            }
        },
            new MyEntity()
{
            Id = Guid.NewGuid().ToString(),
            StringProp = $"New Prop {Random.Next(0, 1000)}",
            IntProp = Random.Next(0,1000),
            IntList =  new List<int>(){
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000),
                Random.Next(0,1000)
            },
            InnerProperty = new MyEntity.InnerClass(){
                    InnerString = $"Random string nubmer {Random.Next(0, 1000)}",
                    StringList = new List<string>(){
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}",
                        $"Random string nubmer {Random.Next(0, 1000)}"
                    }
            }
        },
        });
            await LoadData();

            StateHasChanged();
        }
        private async Task Update(MyEntity entity)
        {
            await _indexedDb.Entities.Update(entity);
        }

        private async Task<ResponseIDB> Remove(MyEntity entity)
        {
            return await _indexedDb.Entities.Remove(entity);
        }

        private async Task RemoveId(string id)
        {
            GetOneEntity = MyEntities.FirstOrDefault(i => i.Id.Equals(id)) ?? new MyEntity();
            var result = await Remove(GetOneEntity);
            if (result.IsSuccess)
            {
                EntityId = "";
                await LoadData();
                StateHasChanged();
            }
        }
        private async Task RemoveRange(ICollection<MyEntity> myEntities)
        {
            var result = await _indexedDb.Entities.RemoveRange(myEntities);
            await LoadData();
            StateHasChanged();
        }
        private async Task UpdateRange(List<MyEntity> myEntities)
        {
            var result = await _indexedDb.Entities.UpdateRange(myEntities);
            await LoadData();
            StateHasChanged();
        }
        private async Task Drop(string tableName)
        {
            var table = _indexedDb?.GetType().GetProperty(tableName);
            var method = table?.PropertyType.GetMethod("DropTable");
            var task = (Task<bool>?)method?.Invoke(null, null);
            if (task is null)
                return;

            bool result = await task;
            await LoadData();
            StateHasChanged();
        }
        void StartedEditingItem(MyEntity item)
        {
        }

        void CancelledEditingItem(MyEntity item)
        {
        }

        async Task CommittedItemChanges(MyEntity item)
        {
            await Update(item);
        }
    }
}
