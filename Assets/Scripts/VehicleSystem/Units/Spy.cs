using System.Collections;
using Core.Characters;

namespace Units
{
    public class Spy : Marine
    {
        private void Update()
        {
            StateMachine.currentState.Update();
        }
        public override void EnterCar()
        {
            gameObject.SetActive(false);
            EventManager.TriggerEvent(GenericEvents.CommonUnitHasEnteredToTheTransport, new Hashtable()
            {
                {GameplayEventHashtableParams.CommonUnitEntered.ToString(),this},
                {GameplayEventHashtableParams.Transport.ToString(),CurrentTransport}
            });
        }
    }
}
