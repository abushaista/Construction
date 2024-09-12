using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure.RabbitMq;

public static class ContextCorrelator
{
    private static readonly AsyncLocal<Dictionary<string, Guid>> _items = new();
    private static Dictionary<string, Guid> Items
    {
        get => _items.Value ??= new Dictionary<string, Guid>();
        set => _items.Value = value;
    }

    public static Guid GetValue(string key)
    {
        return Items.ContainsKey(key) ? Items[key] : Guid.Empty;
    }

    public static IDisposable BeginCorrelationScope(string key, Guid value)
    {
        var scope = new CorrelationScope(Items, LogContext.PushProperty(key, value.ToString("N")));
        if (Items.ContainsKey(key))
        {
            Items[key] = value;
            return scope;
        }

        Items.Add(key, value);
        return scope;
    }

    public sealed class CorrelationScope : IDisposable
    {
        private readonly Dictionary<string, Guid> _bookmark;
        private readonly IDisposable _logContextPop;

        public CorrelationScope(Dictionary<string, Guid> bookmark, IDisposable logContextPop)
        {
            _bookmark = bookmark ?? throw new ArgumentNullException(nameof(bookmark));
            _logContextPop = logContextPop ?? throw new ArgumentNullException(nameof(logContextPop));
        }

        public void Dispose()
        {
            _logContextPop.Dispose();
            Items = _bookmark;
        }
    }
}
