using System.Collections;
using Core.Characters;


namespace Units
{
    public class Pilot : Marine
    {
        private void Update()
        {
            StateMachine.currentState.Update();
        }

        public override void EnterCar()
        {
            gameObject.SetActive(false);
            EventManager.TriggerEvent(GenericEvents.UnitHasEnteredToTheTransport, new Hashtable()
            {
                {GameplayEventHashtableParams.UnitEntered.ToString(),this},
                {GameplayEventHashtableParams.Transport.ToString(),CurrentTransport}
            });
        }
    }
}
