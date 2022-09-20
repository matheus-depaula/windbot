using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;

namespace WindBot.Game.AI.Decks
{
    [Deck("Monarch", "AI_Monarch")]
    public class MonarchExecutor : DefaultExecutor
    {
        public class CardId
        {
            public const int Edea = 95457011;
            public const int Eidos = 59463312;
            public const int Mithra = 22404675;

            public const int Erebus = 23064604;
            public const int Ehther = 96570609;
            public const int MegaCaius = 87288189;
            public const int VanitysFiend = 47084486;
            public const int MajestysFiend = 33746252;
            public const int Kuraz = 57666212;
            public const int Anchamoufrite = 52296675;

            public const int Pantheism = 22842126;
            public const int Domain = 84171830;
            public const int Tenacity = 33609262;
            public const int Stormforth = 79844764;
            public const int Return = 61466310;
            public const int March = 19870120;
            public const int Erupt = 48716527;
            public const int Escalation = 18235309;
            public const int Prime = 54241725;
        }

        private List<int> UsedEffects = new List<int>();
        private List<int> ActiveEffects = new List<int>();
        private bool NormalSummoned = false;
        private bool ExtraTributeSummon = false;

        public MonarchExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // summons
            AddExecutor(ExecutorType.Summon, CardId.Eidos, EidosEffect);
            AddExecutor(ExecutorType.Summon, CardId.Edea, NormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Mithra, NormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Erebus, NormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Ehther, NormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.MegaCaius, NormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.VanitysFiend, NormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.MajestysFiend, NormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Kuraz, NormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Anchamoufrite, NormalSummon);

            // monster effects
            AddExecutor(ExecutorType.Activate, CardId.Edea, EdeaEffect);
            AddExecutor(ExecutorType.Activate, CardId.Eidos, EidosEffect);
            AddExecutor(ExecutorType.Activate, CardId.Mithra, MithraEffect);

            // spell effects
            AddExecutor(ExecutorType.Activate, CardId.Pantheism, PantheismEffect);
        }

        public override bool OnSelectHand()
        {
            // go first
            return true;
        }

        public override void OnNewTurn()
        {
            UsedEffects.Clear();
            ActiveEffects.Clear();
            NormalSummoned = false;
            ExtraTributeSummon = false;
        }

        private bool NormalSummon()
        {
            NormalSummoned = true;
            return true;
        }

        private bool EdeaEffect()
        {
            if (Card.IsDisabled())
                return false;

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Bot.GetRemainingCount(CardId.Eidos, 2) <= 0 && Bot.GetRemainingCount(CardId.Mithra, 1) > 0)
                    AI.SelectCard(CardId.Mithra);
                else
                    AI.SelectCard(CardId.Eidos);

                return true;
            }

            if (Card.Location == CardLocation.Grave)
            {
                if (Bot.HasInBanished(CardId.Pantheism))
                {
                    AI.SelectCard(CardId.Pantheism);
                    return true;
                }

                if (Bot.HasInBanished(CardId.Stormforth))
                {
                    if (Bot.HasInHand(CardId.Ehther))
                    {
                        AI.SelectCard(CardId.Stormforth);
                        return true;
                    }

                    if ((!NormalSummoned ||
                         ExtraTributeSummon ||
                         Bot.HasInHandOrInSpellZone(CardId.Escalation)) &&
                        (Bot.HasInHand(CardId.MegaCaius) ||
                         Bot.HasInHand(CardId.VanitysFiend) ||
                         Bot.HasInHand(CardId.MajestysFiend) ||
                         Bot.HasInHand(CardId.Kuraz))
                       )
                    {
                        AI.SelectCard(CardId.Stormforth);
                        return true;
                    }
                }

                if (Bot.HasInBanished(CardId.Tenacity) &&
                    (Bot.HasInHand(CardId.Erebus) ||
                     Bot.HasInHand(CardId.Ehther) ||
                     Bot.HasInHand(CardId.MegaCaius) ||
                     Bot.HasInHand(CardId.Kuraz) ||
                     Bot.HasInHand(CardId.MajestysFiend))
                   )
                {
                    AI.SelectCard(CardId.Tenacity);
                    return true;
                }

                if (Bot.HasInBanished(CardId.Domain) && !Bot.HasInHandOrInSpellZone(CardId.Domain))
                {
                    AI.SelectCard(CardId.Domain);
                    return true;
                }

                if (Bot.HasInBanished(CardId.Erupt))
                {
                    AI.SelectCard(CardId.Erupt);
                    return true;
                }

                if (Bot.HasInBanished(CardId.Return) && !Bot.HasInHandOrInSpellZone(CardId.Return))
                {
                    AI.SelectCard(CardId.Return);
                    return true;
                }

                if (Bot.HasInBanished(CardId.March))
                {
                    AI.SelectCard(CardId.March);
                    return true;
                }

                if (Bot.HasInBanished(CardId.Escalation))
                {
                    AI.SelectCard(CardId.Escalation);
                    return true;
                }

                if (Bot.HasInBanished(CardId.Prime))
                {
                    AI.SelectCard(CardId.Prime);
                    return true;
                }

                return false;
            }

            return false;
        }

        private bool EidosEffect()
        {
            if (Card.IsDisabled())
                return false;

            if (Card.Location == CardLocation.MonsterZone)
            {
                NormalSummoned = true;
                ExtraTributeSummon = true;
                return true;
            }

            if (Card.Location == CardLocation.Removed)
            {
                AI.SelectCard(Bot.HasInGraveyard(CardId.Edea) ? CardId.Edea : CardId.Mithra);
                return true;
            }

            return false;
        }

        private bool MithraEffect()
        {
            if (Card.IsDisabled())
                return false;

            // TODO: implement mithra extra tribute summon

            return false;
        }

        private bool PantheismEffect()
        {
            if (Card.Location == CardLocation.Grave)
                return PantheismEffectFromGrave();

            if (Card.Location == CardLocation.Hand)
                return PantheismActivateFromHand();

            return false;
        }

        private bool PantheismEffectFromGrave()
        {
            IList<int> targets = new[]
            {
                CardId.Domain,
                CardId.Tenacity,
                CardId.Stormforth,
                CardId.Return,
                CardId.March,
                CardId.Erupt,
                CardId.Escalation
            };

            foreach (int target in targets)
            {
                if (Bot.HasInHandOrInSpellZone(target))
                    targets.Remove(target);
            }

            AI.SelectCard(targets);
            return true;
        }

        private bool PantheismActivateFromHand()
        {
            if (Bot.HasInHand(CardId.Prime))
            {
                AI.SelectCard(CardId.Prime);
                return true;
            }

            if (Bot.HasInHand(CardId.Return))
            {
                if (Bot.Hand.GetCardCount(CardId.Return) > 1)
                {
                    AI.SelectCard(CardId.Return);
                    return true;
                }

                if ((!Bot.HasInHand(CardId.Erebus) ||
                    !Bot.HasInHand(CardId.Ehther) ||
                    !Bot.HasInHand(CardId.MegaCaius) ||
                    !Bot.HasInHand(CardId.MajestysFiend)
                    ) && (!ActiveEffects.Contains(CardId.Eidos) ||
                          !ActiveEffects.Contains(CardId.Mithra))
                    )
                {
                    AI.SelectCard(CardId.Return);
                    return true;
                }
            }

            if (Bot.HasInHand(CardId.Tenacity) && UsedEffects.Contains(CardId.Tenacity))
            {
                if (!Bot.HasInHand(CardId.Erebus) && !Bot.HasInHand(CardId.Ehther) && !Bot.HasInHand(CardId.MegaCaius))
                {
                    AI.SelectCard(CardId.Tenacity);
                    return true;
                }
            }

            if (Bot.HasInHand(CardId.Escalation))
            {
                AI.SelectCard(CardId.Escalation);
                return true;
            }

            if (Bot.HasInHand(CardId.Erupt))
            {
                AI.SelectCard(CardId.Erupt);
                return true;
            }

            if (Bot.HasInHand(CardId.March))
            {
                AI.SelectCard(CardId.March);
                return true;
            }

            return false;
        }
    }
}
