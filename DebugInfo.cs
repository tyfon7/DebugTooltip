using EFT;
using EFT.InventoryLogic;
using System.Text;

using SlotItemAddress = GClass2783;

namespace DebugTooltip
{
    public abstract class DebugInfo() 
    {
        public virtual string ToShortString()
        {
            return ToString();
        }
    }

    public class ItemDebugInfo(Item item) : DebugInfo
    {
        private readonly Item item = item;

        public override string ToShortString()
        {
            return item?.Id;
        }

        public override string ToString()
        {
            if (item == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new();

            sb.AppendFormat("ID: <color=yellow>{0}</color>\n", item.Id);
            sb.AppendFormat("Tpl: <color=green>{0}</color>\n", item.TemplateId);
            sb.AppendFormat("Owner: <color=#ffa500>{0}</color>\n", item.Owner.ContainerName);
            sb.AppendFormat("<color=#ffa500>{0}</color>\n", item.Owner.ID);

            if (item.CurrentAddress is ItemAddressClass gridAddress)
            {
                sb.AppendFormat("Address: <color=#00ffff>({0}, {1}) {2}</color>\n", gridAddress.LocationInGrid.x, gridAddress.LocationInGrid.y, gridAddress.LocationInGrid.r);
                sb.AppendFormat("<color=#00ffff>Grid {0}</color>\n", gridAddress.Grid.ID);
                sb.AppendFormat("<color=#00ffff>{0}</color>\n", gridAddress.Grid.ParentItem.Name.Localized());
                sb.AppendFormat("<color=#00ffff>{0}</color>\n", gridAddress.Grid.ParentItem.Id);
            }
            else if (item.CurrentAddress is SlotItemAddress slotAddress)
            {
                sb.AppendFormat("Slot: <color=#00ffff>{0}</color>\n", slotAddress.Slot.ID);
                sb.AppendFormat("<color=#00ffff>{0}</color>\n", slotAddress.Slot.ParentItem.Name.Localized());
                sb.AppendFormat("<color=#00ffff>{0}</color>\n", slotAddress.Slot.ParentItem.Id);
            }
            else
            {
                sb.AppendFormat("Address: <color=red>{0}</color>\n", item.CurrentAddress.ToString());
            }

            return sb.ToString();
        }
    }

    public class EmptySlotDebugInfo(Slot slot) : DebugInfo
    {
        private readonly Slot slot = slot;

        public override string ToShortString()
        {
            return slot?.ID;
        }

        public override string ToString()
        {
            if (slot == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new();

            sb.AppendFormat("Empty slot: <color=#00ffff>{0}</color>\n", slot.ID);
            sb.AppendFormat("<color=#00ffff>{0}</color>\n", slot.ParentItem.Name.Localized());
            sb.AppendFormat("<color=#00ffff>{0}</color>\n", slot.ParentItem.Id);

            return sb.ToString();
        }
    }

    public class TraderDebugInfo(Profile.TraderInfo traderInfo) : DebugInfo
    {
        private readonly Profile.TraderInfo traderInfo = traderInfo;

        public override string ToShortString()
        {
            return traderInfo?.Id;
        }

        public override string ToString()
        {
            if (traderInfo == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new();

            sb.AppendFormat("ID: <color=yellow>{0}</color>\n", traderInfo.Id);

            return sb.ToString();
        }
    }

    public class QuestDebugInfo(QuestClass quest) : DebugInfo
    {
        private readonly QuestClass quest = quest;

        public override string ToShortString()
        {
            return quest?.Id;
        }

        public override string ToString()
        {
            if (quest == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new();

            sb.AppendFormat("ID: <color=yellow>{0}</color>\n", quest.Id);
            sb.AppendFormat("Status: <color=#00ffff>{0}</color>\n", quest.QuestStatus);

            return sb.ToString();
        }
    }
}
