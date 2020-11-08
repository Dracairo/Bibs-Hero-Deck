using System;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Mail;
using System.Linq;

namespace Workshopping.Bibs
{
    public class BibsCharacterCardController : HeroCharacterCardController
    {

        public BibsCharacterCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            //Discard Card.
            List<DiscardCardAction> storedResults = new List<DiscardCardAction>();
            IEnumerator coroutine = base.GameController.SelectAndDiscardCard(this.DecisionMaker, true, null, storedResults, SelectionType.DiscardCard, null, base.TurnTaker, false, null, base.GetCardSource(null));

            //If you do, 1 player may play a card or draw a card.
            {
                if (!base.DidDiscardCards(storedResults, new int?(1), false))
                {
                    coroutine = base.GameController.SendMessageAction(base.TurnTaker.Name + " did not discard a card, so no card will be played or drawn.", Priority.Medium, base.GetCardSource(null), null, true);
                }
                else
                {
                    //Create list of Functions
                    List<Function> list = new List<Function>();

                    //Add play a card option
                    list.Add(new Function(this.DecisionMaker, "Play a card.", SelectionType.PlayCard, () => base.SelectAndPlayCardFromHand(base.HeroTurnTakerController, true, null, null, false, false, false, "Play a card.")));

                    //Add draw a card option
                    list.Add(new Function(this.DecisionMaker, "Draw a card.", SelectionType.DrawCard, () => base.DrawCard(null, false, null, true), null, "Draw a card.", null));

                    //Have player choose which option to do
                    SelectFunctionDecision selectFunction = new SelectFunctionDecision(base.GameController, this.DecisionMaker, list, false, null, null, null, base.GetCardSource(null));

                    //Perform the Selected Option
                    IEnumerator performFunction = base.GameController.SelectAndPerformFunction(selectFunction, null, null);
                }
            }
            return base.UsePower(index);
        }
        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        //One player may draw 3 cards.
                        {
                            List<DrawCardAction> storedResults = new List<DrawCardAction>();
                            IEnumerator coroutine = base.GameController.SelectHeroToDrawCards(base.HeroTurnTakerController, 3, true, false, null, true, null);

                            //If they do, they must discard 1 card.
                            if (base.DidDrawCards(storedResults, new int?(1)))
                            {
                                IEnumerator coroutine2 = base.GameController.SelectAndDiscardCard(HeroTurnTakerController, false, null, null, SelectionType.DiscardCard, null, null, false, null, base.GetCardSource(null));
                            }

                        }
                        break;
                    }
                case 1:
                    {
                        //One hero deals 1 target 3 melee damage.
                        IEnumerator coroutine = base.GameController.SelectHeroToSelectTargetAndDealDamage(this.DecisionMaker, 3, DamageType.Melee, false, false, true, false, null, base.GetCardSource(null));

                        break;
                    }
                case 2:
                    {
                        //Choose a Target. Reduce damage dealt by that target by 2 until the start of your turn.
                        List<SelectCardDecision> storedResults = new List<SelectCardDecision>();

                        IEnumerator coroutine = base.GameController.SelectCardAndStoreResults(this.DecisionMaker, SelectionType.SelectTargetNoDamage, new LinqCardCriteria((Card c) => c.IsInPlay && c.IsTarget, "target", false, false, null, "targets", false), storedResults, false, false, null, true, base.GetCardSource(null));
                        SelectCardDecision selected = storedResults.FirstOrDefault<SelectCardDecision>();
                        if (selected != null && selected.SelectedCard != null)
                        {
                            ReduceDamageStatusEffect reduceDamageStatusEffect = new ReduceDamageStatusEffect(1);
                            reduceDamageStatusEffect.SourceCriteria.IsSpecificCard = selected.SelectedCard;
                            reduceDamageStatusEffect.UntilStartOfNextTurn(base.TurnTaker);
                            reduceDamageStatusEffect.UntilTargetLeavesPlay(selected.SelectedCard);
                            coroutine = base.AddStatusEffect(reduceDamageStatusEffect, true);
                        }
                        break;
                    }
            }
            return base.UseIncapacitatedAbility(index);
        }
    }
}