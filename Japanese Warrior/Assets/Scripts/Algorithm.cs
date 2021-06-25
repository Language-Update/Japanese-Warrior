using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour{

	// Here you can find how algorithm works by it's explaination, psuedo code, values, and actual code

	/*
	 * Content Properties  AP = Acquisition Point, AC = Acquisition Category
		=========================================================================================
		- Language: EN-JP
		- Content Type: Letter / Word / Phrase / Sentence ....
		- Content ID: 4481001245 (First 4 number is the country code on the languages. 44 for the UK, 81 for Japan)
		- AP = 1
		- AP Multiplier = 0.1
		- AC = "New"; (weak, moderate, good, perfect, native)
		- numberOfTest = 0; // This shows how many times user answered the question
		- (Gelecek) > Confuse <Dictinary>() (ContentID : ConfuseIndex)	// When user choose/select wrong content, we add that content to the confuse dic for the asked content. If there already is the confused content, we increase the index by 1)

		=========================================================================================

		Question Types and multipliers

		- Multiple Choice: 1 
		- Write:   1.5
		- Read:    1.3
		- Sorting: 1.1


		Check After

		- Native:   200 tests
		- Perfect:  100 tests
		- Good:     50  tests
		- Moderate: 25  tests
		- Weak:     12  tests


		Rate Of Repetation Type

		- New:		30%
		- Weak:		25%
		- Moderate:	20%
		- Good:		14%
		- Perfect:	8%
		- Native:	3%



		AC Options related to the number of tests

		if number of test >= *value* execute: if AP >= *value* : AC = *result*
		number of tests >= 200 : 10.000: Native, 2.000: Perfect, 500: Good, 100: Moderate, else Weak	| +%90-80-70-60-
		number of tests >= 100 : 700: Native, 210: Perfect, 90: Good, 28: Moderate, else Weak 		| +%95-80-70-60-
		number of tests >= 50  : 90: Perfect, 55: Good, 12: Moderate, else Weak				| +%90-80-60-
		number of tests >= 25  : 20: Good, 6: Moderate, else Weak					| +%80-60-
		number of tests >= 12  : 4: Moderate, else Weak							| +%60-
		number of tests >= 0   : New

		=========================================================================================

		Acquisition Point (AP) Algorithm

		- AP :1
		- AP Multiplier: 0.1 (increases .05 every time)
			10 True: 0.6
			20 True: 1.1
			30 True: 1.6
			40 True: 2.1

		After Question

		- True: 
			content.AP += questionMultiplier * 2^content.APmultiplier;
			content.APmultiplier += 0.1f;

		- False:
			content.AP -= questionMultiplier * 2^content.APmultiplier;
			content.APmultiplier -= 0.1f;



		=========================================================================================

		AC_Type_Selection_Method(int Total_Needed_Content)

		// Define how many content you need for each type

		Needed_Native_Count = Total_Needed_Content * (3/100);		// Needed_ContentType_Count's are class's own variables not methods
		Needed_Perfect_Count = Total_Needed_Content * (8/100);
		Needed_Good_Count = Total_Needed_Content * (14/100);
		Needed_Moderate_Count = Total_Needed_Content * (20/100);
		Needed_Weak_Count = Total_Needed_Content * (25/100);
		Needed_New_Count = Total_Needed_Content * (30/100);

		// If user has less then needed, then transfer the difference to the lower type and take what User's have

		if ( User's_Native_Count < Needed_Native_Count ) then			// 
			Needed_Perfect_Count += Needed_Native_Count - User's_Native_Count;
			Needed_Native_Count = User's_Native_Count;

		if ( User's_Perfect_Count < Needed_Perfect_Count ) then	
			Needed_Good_Count += Needed_Perfect_Count - User's_Perfect_Count;	// Transfer the difference
			Needed_Perfect_Count = User's_Perfect_Count;						// Take the remaining

		If ( ..... Same goes for other types: Good, Moderate, Weak, New);

		// Now you now how many content you can get for each type. If there are lots of content for every type which fulfills the request, then you will get as many as you defined initially. 
		// If there is no or not enough for the highest type, the difference between needed and current supply goes the lower type and you get what user have. 

		// For Example if algorithm need 5 Native, 12 Perfect, 20 Good, 35 Moderate, 45 Weak, 60 New content. If user only have 3 Native content, then algotihm pick all of those 3
			and transfer remainin 2 to Needed Perfect Content. So, Algorithm will look for 14 Perfect content instead of 12. If there is not enough of them, then same applies here again.
			Therefore algorithm will keep content rate constant as long as there is enough of them. The rate is defined above as "Rate Of Repetation Type"
	*/










}
