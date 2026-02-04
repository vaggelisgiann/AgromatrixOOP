using System.Text.Json;
using Agromatrix.Models;
using Microsoft.JSInterop;

namespace Agromatrix.Services
{
    public class CartService
    {
        private const string StorageKey = "agromatrix_cart_v1";
        private readonly IJSRuntime _js;
        private readonly List<CartItem> _items = new();

        public IReadOnlyList<CartItem> Items => _items;
        public event Action? OnChange;

        public CartService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var json = await _js.InvokeAsync<string>("localStorage.getItem", StorageKey);
                if (!string.IsNullOrEmpty(json))
                {
                    var loaded = JsonSerializer.Deserialize<List<CartItem>>(json);
                    if (loaded != null)
                    {
                        _items.Clear();
                        _items.AddRange(loaded);
                        NotifyStateChanged();
                    }
                }
            }
            catch
            {
                // ignore localStorage errors
            }
        }

        public async Task AddToCartAsync(Product product, int quantity)
        {
            if (product == null || quantity <= 0) return;
            var existing = _items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing != null) existing.Quantity += quantity;
            else _items.Add(new CartItem {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity
            });
            await SaveAsync();
            NotifyStateChanged();
        }

        public async Task RemoveFromCartAsync(int productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null) { _items.Remove(item); await SaveAsync(); NotifyStateChanged(); }
        }

        public async Task UpdateQuantityAsync(int productId, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0) _items.Remove(item);
                else item.Quantity = quantity;
                await SaveAsync();
                NotifyStateChanged();
            }
        }

        public async Task ClearAsync()
        {
            _items.Clear();
            await SaveAsync();
            NotifyStateChanged();
        }

        private async Task SaveAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(_items);
                await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
            }
            catch
            {
                // ignore
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}