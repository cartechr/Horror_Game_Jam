using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{

	public class Drawer_Pull_X : MonoBehaviour
	{

		Animator animator;
		GameObject Player;

		bool canInteract = true;
		bool isOpen = false;


        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        private void Update()
        {
            if (Player == null)
			{
				Player = GameObject.FindGameObjectWithTag("Player");
			}
        }

        void OnMouseOver()
		{
			Debug.Log("See Drawer");
			if (Player)
			{
				if (canInteract)
				{
					Debug.Log("Can Interact");
					if (Vector3.Distance(transform.position, Player.transform.position) < 3f)
					{
						Debug.Log("Can Open/Close");

						if (Input.GetKeyDown(KeyCode.E))
						{
							Debug.Log("Opening/Closing Drawer");

							if (isOpen)
							{
								//Close Drawer
								isOpen = false;
								closeDrawer();
							}
							else
							{
								//Open Drawer
								isOpen = true;
								openDrawer();
							}

							canInteract = false;
						}
					}
				}
			}



			


		}

		void openDrawer()
		{
			animator.SetBool("Open", true);
		}

		void closeDrawer() 
		{
			animator.SetBool("Close", true);
		}

		void finishedOpening()
		{
			canInteract = true;
			animator.SetBool("Open", false);
		}

		void finishedClosing()
		{
			canInteract = true;
			animator.SetBool("Close", false);
		}


	}
}