using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System.Collections;



namespace Workshopping.Bibs
{
    public class FlairController : CardController
    {
        public FlairController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
        public override IEnumerator ActivateAbility(string abilityKey)
        {
            IEnumerator enumerator = null;
           if (abilityKey == "flair")
            {
                enumerator = this.ActivateFlair();
            }
            return base.ActivateAbility(abilityKey);
        }
    
        //Activate Flair texts.
        public virtual IEnumerator ActivateFlair()
        {
            yield return null;
            yield break;
        }
    }
 }
