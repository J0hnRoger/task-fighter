using System.Reflection;
using Spectre.Console;
using TaskFighter.Domain.Common.Interfaces;

namespace TaskFighter.Infrastructure.Renderer;

    public class TaskFighterTable<T> where T : ITableRenderable
    {
        private Table _table;

        public Table Table
        {
            get
            {
                return _table;
            }
        }

        public TaskFighterTable(List<T> iterables, List<string>? fields = null)
        { 
            _table = new Table();
            _table.Border = TableBorder.Rounded;

            if (fields == null)
               fields = typeof(T).GetProperties().Select(p => p.Name).ToList();
            
            // baseclass property 
            _table.AddColumn(new TableColumn("[green]Id[/]"));
            foreach (string field in fields)
            {
                _table.AddColumn(new TableColumn($"[green]{field}[/]").LeftAligned());
            }

            foreach (var row in iterables)
            {
                _table.AddRow(row.GetFields().Take(fields.Count).ToArray());
            }
        }
    }
