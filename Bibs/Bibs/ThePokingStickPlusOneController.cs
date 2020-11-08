using System;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System.Collections;

namespace Workshopping.Bibs
{
	public class ThePokingStickPlusOneController : CardController
	{

		public ThePokingStickPlusOneController(Card card, TurnTakerController turnTakerController)
			: base(card, turnTakerController)
		{
		}
		//Increase Melee Damage dealt by Bibs by 1.
		public override void AddTriggers()
		{
			base.AddIncreaseDamageTrigger((DealDamageAction dd) => dd.DamageSource.IsSameCard(base.CharacterCard) && dd.DamageType == DamageType.Melee, 1, null, null, false);
		}
        //Power: Bibs deals 1 target 2 melee damage.
        public override IEnumerator UsePower(int index = 0)
		{
			IEnumerator coroutine = base.GameController.SelectTargetsAndDealDamage(this.DecisionMaker, new DamageSource(this.GameController, this.CharacterCard), 2, DamageType.Melee, 1, false, 1, cardSource:(GetCardSource()));

			return base.UsePower(index);
		}
	}
}