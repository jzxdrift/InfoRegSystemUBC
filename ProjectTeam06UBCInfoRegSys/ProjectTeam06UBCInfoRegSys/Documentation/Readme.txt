Getting started tips:
1. Make sure Entity Framework package for this solution is installed on your PC.
2. Delete Migrations folder.
3. After enable-migrations command in Package Manager Console, the Configuration.cs file will open.
   Replace the code in this file with code provided in Documentation folder -> Configuration.txt file
   and run update-database command in Package Manager Console.
4. Open UBCMainForm.cs -> View Code. Uncomment line 31 and run the application. This will seed the database
   with data provided in bin\Debug\Data. Important: comment line 31 back before second and further application runs.

Application usage tips:
On the main screen, select the user you want to proceed as and click login.

Student user tips:
1. Select option to proceed with.
2. Courses can be filtered by department name.
3. If no department name is selected, all courses will be shown after apply is clicked.
4. Instructors can be filtered by department name, combination of first and last names, or both.
5. If no department name is selected and no first and last names are entered,
   all instructors will be shown after apply is clicked.
6. Registrations can be filtered by both department name and course ID only.
7. If no department name is selected and no course ID is entered, all registrations will be shown after apply is clicked.
8. We assume that students know their IDs and names, so in order to register, all data should be entered manually,
   EXCEPT registration ID (auto-incremented identity). Applying filters first and then register will make things easier.
   Once all data is entered, select the entire row and click register. A confirmation message will pop up in case of
   successful registration, otherwise error message will be displayed.

Instructor user tips:
1. Departments, courses, instructors, and registrations options work the same way as in student user mode,
   EXCEPT instructor user can not register for courses.
2. Students can be filtered by department name, combination of first and last names, or both.
3. If no department name is selected and no first and last names are entered,
   all students will be shown after apply is clicked.

Guest user tips:
1. Departments, courses, and instructors options work the same way as in student and instructor user modes.

Admin user tips:
1. Courses filters work the same way as in student, instructor, and guest user modes.
2. In order to add course, all data should be entered manually. Applying filters first and then add
   will make things easier. Once all data is entered, select the entire row and click add. A confirmation message will
   pop up in case of success, otherwise error message will be displayed.
3. In order to delete course, select the entire row and click delete. A confirmation message will pop up in case of
   success, otherwise error message will be displayed.
4. Sections can be filtered by both department name and course ID only.
5. If no department name is selected and no course ID is entered, all sections will be shown after apply is clicked.
6. In order to add section, all data should be entered manually. Applying filters first and then add
   will make things easier. Once all data is entered, select the entire row and click add. A confirmation message will
   pop up in case of success, otherwise error message will be displayed.
7. In order to delete section, select the entire row and click delete. A confirmation message will pop up in case of
   success, otherwise error message will be displayed.

P.S. If you have any problems or further questions, feel free to contact Albert Muller directly either at his
	 college (mullera@student.douglascollege.ca) or personal email (giga.asd@gmail.com).
