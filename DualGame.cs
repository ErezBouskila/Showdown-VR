using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * Milestones:
 * 1. Scene with placeholder.
 * 2. Basic game states with ready state.
 * 3. Shoot logic.
 * 4. 
 * .
 * 7. Cardboard SDK
 * .
 * 10. Add real models.
 * 11. Animations.
 * 12. Make a lot of money.
 * 
 */

public class DualGame : MonoBehaviour
{
	public enum State
	{
		Idle,
		Ready,
		CountDownBeforeRandomShoot,
		ShootDuringCountDown,
		RandomShootCountDown,
		ShootNow,
		PauseBeforeResult,
		Result
	}

	public enum Result
	{
		Missed,
		Win,
		Lost,
	}


	public State m_state;
	public int m_counterdown;
	public int m_randomShootIndicator;
	public float m_timeTookPlayerToShoot;
	public Result m_result;
	public Text m_ready;
	public Text m_set;
	public Text m_shootIndicator;
	public Text m_win;
	public Text m_lose;
	public Text m_miss;
	public Text m_shootTime;
//	public GameObject playerCanvas;

	private float m_timer;

	private const int COUNTDOWN_STEPS = 2;
	private const float TIME_TO_SHOOT = 0.400f;

	// Use this for initialization
	void Start () {
		m_state = State.Idle;
		m_shootIndicator.text = "";
		m_ready.text = "";
		m_set.text = "";
		m_win.text = "";
		m_lose.text = "";
		m_miss.text = "";
	}
	
	// Update is called once per frame
	void Update () {

		UpdateState ();


		if (Input.GetButtonDown("Fire1")) {
			switch (m_state) {
			case State.Idle:
				{
					SetState (State.Ready);
					break;
				}
			case State.Result:
				{
					SetState (State.Idle);
					break;
				}
			}
		}

	}


	void UpdateState()
	{
		m_timer += Time.deltaTime;

		switch (m_state) {
		case State.Ready:
			{
				m_ready.text = "Ready";
				SetState (State.CountDownBeforeRandomShoot);
				break;
			}
		case State.CountDownBeforeRandomShoot:	
			{
				//Shoot before end of CountDown
				if (Input.GetButtonDown("Fire1")) {
					m_ready.text = "";
					m_set.text = "";
					m_miss.text = "Missed";
					m_result = Result.Missed;
					SetState (State.PauseBeforeResult);
				}

				if (m_timer >= 1) {
					m_counterdown--;
					m_timer = 0;
				}

				if (m_counterdown == 0) {
					m_ready.text = "";
					m_set.text = "set";
					SetState (State.RandomShootCountDown);
				}
				break;
			}
		case State.RandomShootCountDown:	
			{
				//Shoot before end of CountDown
				if (Input.GetButtonDown("Fire1")) {
					m_shootIndicator.text = "";
					m_miss.text = "Missed";
					m_result = Result.Missed;
					SetState (State.PauseBeforeResult);
				}

				if (m_timer >= 1) {
					m_set.text = "";
					m_counterdown--;
					m_timer = 0;
				}

				if (m_counterdown == -1) {
					SetState (State.ShootNow);
				}
				break;
			}
		case State.ShootNow:
			{
//				m_shootIndicator.text = "Shoot";
				//Shoot before the time limit
				if (Input.GetButtonDown ("Fire1")) {
					m_timeTookPlayerToShoot = m_timer;
					if (m_timeTookPlayerToShoot <= TIME_TO_SHOOT) {
						m_shootIndicator.text = "";
						m_win.text = "Win";
						Debug.Log ("TimeTookPlayerToShoot - WaitinForShot " + m_timeTookPlayerToShoot);
						m_result = Result.Win;
						SetState (State.PauseBeforeResult);
					}
				}

				if (m_timer > TIME_TO_SHOOT) {
					m_shootIndicator.text = "";
					m_lose.text = "Lose";
					m_result = Result.Lost;
					Debug.Log ("TimeTookPlayerToShoot - WaitinForShot " + m_timeTookPlayerToShoot);
					SetState (State.PauseBeforeResult);
				}
				break;
			}
		case State.PauseBeforeResult:
			{	
				if (m_timer >= 2) {
					//m_result = Result.Result;
					SetState (State.Result);
				}
				break;
			}
		}
	}


	void SetState (State newState)
	{
		Debug.Log ("Settings state from " + m_state + " to " + newState);
//		m_shootTime.text = "Settings state from " + toString(m_state) + " to " + newState
		
		switch (newState) {
		case State.Idle:
			{
				m_timeTookPlayerToShoot = 0;
				break;
			}
		case State.CountDownBeforeRandomShoot:
			{
				m_counterdown = COUNTDOWN_STEPS;
				break;
			}
		case State.RandomShootCountDown:
			{
				m_randomShootIndicator = Random.Range (1, 3);
				m_counterdown = m_randomShootIndicator;
				break;
			}
		case State.ShootNow:
			{
				m_shootIndicator.text = "Shoot";
				break;
			}
		case State.Result:
			{
				Debug.Log (State.Result);
				m_shootIndicator.text = "";
				m_win.text = "";
				m_lose.text = "";
				m_miss.text = "";
				Debug.Log ("TimeTookPlayerToShoot - Result" + m_timeTookPlayerToShoot);
				break;
			}
		}
		m_timer = 0;
		m_state = newState;
	}

//	public static Text AddTextToCanvas(string textString, GameObject canvasGameObject)
//	{
//		Text text = canvasGameObject.AddComponent<Text>();
//		text.text = textString;
//
//		Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
//		text.font = ArialFont;
//		text.material = ArialFont.material;
//
//		return text;
//	}
}
