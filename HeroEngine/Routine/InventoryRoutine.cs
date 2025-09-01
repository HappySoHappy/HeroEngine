using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Request;

namespace HeroEngine.Routine
{
    public class InventoryRoutine : Routine<RoutineResult>
    {
        protected ExecutionConfiguration _config;
        public InventoryRoutine(Account account, ExecutionConfiguration config) : base(account)
        {
            _config = config;
        }

        public override bool Execute(out RoutineResult result, out string error)
        {
            if (_config.InventorySellPreferredSelect == ExecutionConfiguration.InventorySellSelect.None)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            var data = _account.HeroZero!.Data;

            List<Item> itemsInInventory = data.Items.Where(item => data.Inventory.GetInventoryItemIds(data.Character.Level).Contains(item.Id)).ToList();
            //for items that are not sellable, try to move them to the storage unit (if it has space)
            //for pets, if claim pets is true, just claim them, and set sidekick to the one before we claimed it
            //if we have throwables and they are not equipped, equip them to save space aswell

            var sellableItems = itemsInInventory.Where(item => item.Type <= 7); // items that are Mask, Cape, Suit, Belt, Boots, Weapon or a Gadget type

/*
            #region Fitler unequipped types
            //add config property to specify if we should keep item types that we dont have equipped
            if (data.Inventory.Mask == 0)
            {
                var keepItem = sellableItems
                    .Where(item => item.IsMaskItem())
                    .OrderByDescending(item => item.Strength + item.Stamina + item.DodgeRating + item.CriticalRating + item.WeaponDamage)
                    .FirstOrDefault();

                sellableItems = sellableItems.Where(item => !item.IsMaskItem() || item.Id == keepItem?.Id);
            }
            #endregion

            switch (_config.InventorySellPreferredSelect)
            {
                case ExecutionConfiguration.InventorySellSelect.Worse:
                    for (int type = 1; type <= 7; type++)
                    {
                        int equippedItemId = type switch
                        {
                            1 => data.Inventory.Mask,
                            2 => data.Inventory.Cape,
                            3 => data.Inventory.Suit,
                            4 => data.Inventory.Belt,
                            5 => data.Inventory.Boots,
                            6 => data.Inventory.Weapon,
                            7 => data.Inventory.Gadget,
                            _ => 0
                        };

                        if (equippedItemId == 0) continue; // No item equipped for this type

                        var equippedItem = data.Items.FirstOrDefault(item => item.Id == equippedItemId);
                        if (equippedItem == null) continue;

                        sellableItems = sellableItems.Where(item =>
                            item.Type != type || IsFirstItemBetter(equippedItem, item)
                        );
                    }
                    break;

                case ExecutionConfiguration.InventorySellSelect.All:
                    break;
            }*/

            foreach (var item in sellableItems)
            {
                if (new SellInventoryItem(_account, item.Id).Execute(out var sellData, out string sellError))
                {
                    _account.Logger.Info($"Sold {item.Identifier} for {item.SellValue} gold coins");
                    SellInventoryItem.Update(_account, sellData);
                } else
                {
                    _account.Logger.Warn($"Unable to sell item {item.Identifier}, {sellError}");
                }
            }

            //find items in inventory, and equip those that are better

            result = RoutineResult.Finished;
            error = "";
            return true;
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new InventoryRoutine(account, config).Execute(out result, out error);
        }

        private bool IsFirstItemBetter(Item first, Item second)
        {
            var improvement = _account.HeroZero!.Data.ItemImprovements.Find(improvement => improvement.ItemId == first.Id);
            int improvementBonus = improvement != null ? (improvement.Strength + improvement.Stamina + improvement.DodgeRating + improvement.CriticalRating) : 0;

            var improvementSecond = _account.HeroZero!.Data.ItemImprovements.Find(improvement => improvement.ItemId == second.Id) ?? improvement;
            int improvementSecondBonus = improvementSecond != null ? (improvementSecond.Strength + improvementSecond.Stamina + improvementSecond.DodgeRating + improvementSecond.CriticalRating) : improvementBonus;

            return (first.Strength + first.Stamina + first.DodgeRating + first.CriticalRating + first.WeaponDamage + improvementBonus) > (second.Strength + second.Stamina + second.DodgeRating + second.CriticalRating + second.WeaponDamage + improvementSecondBonus);
        }

        /*
         * P.itemSellPrice = function(a, b) {
    var c = x.get_item_sell_price_percentage();
    1 == b ? (b = x.get_item_sell_price_premium_factor(), a = a * c * b) : a *= c;
    return Math.round(a)
};
         */

        /*
         * Cd.prototype.get_sellPrice = function() {
    var a = !1;
    jc.fromId(this.get_identifier()).get_premiumOnly() && (a = !0);
    var b = this.get_statBaseStamina(),
        c =
        this.get_statBaseStrength(),
        d = this.get_statBaseCriticalRating(),
        e = this.get_statBaseDodgeRating(),
        l = this.get_itemImprovement();
    null != l && (b -= l.get_statStamina(), c -= l.get_statStrength(), d -= l.get_statCriticalRating(), e -= l.get_statDodgeRating());
    c = P.getNewSidekickStatValue(this.get_quality(), b, this.get_level()) + P.getNewSidekickStatValue(this.get_quality(), c, this.get_level()) + P.getNewSidekickStatValue(this.get_quality(), d, this.get_level()) + P.getNewSidekickStatValue(this.get_quality(), e, this.get_level());
    c = Math.ceil(c / x.get_item_sidekick_sell_price_divider());
    c = q.parseInt(q.string(P.itemBuyPrice(this.get_quality(), c, 1) * x.get_item_sidekick_buy_price_factor()));
    b = P.itemSellPrice(c, a);
    a = 0;
    null != l && (a = !1, c = l.get_statStamina() + l.get_statStrength() + l.get_statCriticalRating() + l.get_statDodgeRating(), c = Math.ceil(c / x.get_item_sidekick_sell_price_divider()), c = P.itemBuyPrice(l.get_quality(), c, 1), a = P.itemSellPrice(c, a));
    l = (b + a) * x.get_item_sidekick_sell_price_multiplier();
    return Math.round(l)
};
         */
    }
}
