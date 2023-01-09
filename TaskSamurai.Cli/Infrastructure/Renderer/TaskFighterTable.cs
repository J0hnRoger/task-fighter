using System.Reflection;
using Spectre.Console;
using TaskSamurai.Domain.Common.Interfaces;

namespace TaskSamurai.Infrastructure.Renderer;

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

        public TaskFighterTable(List<T> iterables)
        { 
            _table = new Table();
            _table.Border = TableBorder.Rounded;

            PropertyInfo[] properties = typeof(T).GetProperties();
            // baseclass property 
            _table.AddColumn(new TableColumn("[green]Id[/]"));
            foreach (PropertyInfo propertyInfo in properties)
            {
                _table.AddColumn(new TableColumn($"[green]{propertyInfo.Name}[/]").LeftAligned());
            }

            foreach (var row in iterables)
            {
                _table.AddRow(row.GetFields());
            }
        }
    }
