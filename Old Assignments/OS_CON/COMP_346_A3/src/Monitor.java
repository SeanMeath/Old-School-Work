/**
 * Class Monitor
 * To synchronize dining philosophers.
 *
 * @author Serguei A. Mokhov, mokhov@cs.concordia.ca
 */
public class Monitor
{
	/*
	 * ------------
	 * Data members
	 * ------------
	 */

	private static boolean[] utensils;
	private static boolean talking = false;

	/**
	 * Constructor
	 */
	public Monitor(int piNumberOfPhilosophers)
	{
		// TODO: set appropriate number of chopsticks based on the # of philosophers
		utensils = new boolean[piNumberOfPhilosophers];
	}

	/*
	 * -------------------------------
	 * User-defined monitor procedures
	 * -------------------------------
	 */

	/**
	 * Grants request (returns) to eat when both chopsticks/forks are available.
	 * Else forces the philosopher to wait()
	 */
	public synchronized void pickUp(final int piTID)
	{
		try {
			while (checkAndSetUtensils(piTID)){
				wait();
			}
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
	}

	/**
	 * When a given philosopher's done eating, they put the chopstiks/forks down
	 * and let others know they are available.
	 */
	public synchronized void putDown(final int piTID)
	{
		int leftUtensil = piTID % utensils.length;
		int rightUtensil = (piTID + 1) % utensils.length;

		utensils[leftUtensil] = false;
		utensils[rightUtensil] = false;
		notifyAll();
	}

	/**
	 * Checks and sets the utensil bool atonimically
	 */
	public synchronized boolean checkAndSetUtensils(int piTID)
	{
		int leftUtensil = piTID % utensils.length;
		int rightUtensil = (piTID + 1) % utensils.length;

		if(utensils[leftUtensil] || utensils[rightUtensil])
			return true;

		utensils[leftUtensil] = true;
		utensils[rightUtensil] = true;
		return false;
	}

	/**
	 * Only one philopher at a time is allowed to philosophy
	 * (while she is not eating).
	 */
	public synchronized void requestTalk()
	{
		try {
			while (checkAndSetTalk()){
				wait();
			}
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
	}

	/**
	 * When one philosopher is done talking stuff, others
	 * can feel free to start talking.
	 */
	public synchronized void endTalk()
	{
		talking = false;
		notifyAll();
	}

	/**
	 * Checks and sets the talking bool atonimically
	 */
	public synchronized boolean checkAndSetTalk()
	{
		if(talking)
			return true;
		talking = true;
		return false;
	}
}

// EOF
