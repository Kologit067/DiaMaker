using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaMakerModel
{
    public class TableDto
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsColored { get; set; }
        public int Group { get; set; }
        public List<ForeignKeyDto> ForeignKeysFrom { get; set; }
        public List<ForeignKeyDto> ForeignKeysTo { get; set; }
        public TableDto()
        {
            IsColored = true;
            ForeignKeysFrom = new List<ForeignKeyDto>();
            ForeignKeysTo = new List<ForeignKeyDto>();
        }
        public TableDto(Table table)
        {
            Name = table.Name;
            IsSelected = table.IsSelected;
            IsColored = table.IsColored;
            Group = table.Group;
            ForeignKeysFrom = new List<ForeignKeyDto>();
            ForeignKeysTo = new List<ForeignKeyDto>();
        }
    }
}
