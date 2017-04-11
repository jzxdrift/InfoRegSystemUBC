using System;
using System.Windows.Forms;

namespace ProjectTeam06UBCInfoRegSys.AppForms
{
    /// <summary>
    /// Represents options to proceed with.
    /// </summary>
    public enum Options
    {
        Departments,
        Courses,
        Instructors,
        Students,
        Registrations,
        Add_Delete_Course,
        Add_Delete_Section,
    }

    public partial class UBCOptionsForm : Form
    {
        private Users user; //current system user

        public UBCOptionsForm(Users user)
        {
            InitializeComponent();
            this.user = user;

            //modify form according to selected user
            switch(this.user)
            {
                case Users.Guest:
                    CreateGuestForm();
                    break;
                case Users.Student:
                    CreateStudentForm();
                    break;
                case Users.Instructor:
                    CreateInstructorForm();
                    break;
                case Users.Admin:
                    CreateAdminForm();
                    break;
            } //end switch

            //select departments by default
            comboBoxOptions.SelectedItem = Options.Departments;
        }

        #region Form modifying methods

        /// <summary>
        /// Create new Guest Form method.
        /// </summary>
        private void CreateGuestForm()
        {
            //populate drop down menu with guest options
            comboBoxOptions.Items.Add(Options.Departments);
            comboBoxOptions.Items.Add(Options.Courses);
            comboBoxOptions.Items.Add(Options.Instructors);

            //change form text
            Text = "UBC - Guest";
        }

        /// <summary>
        /// Create new Student Form method.
        /// </summary>
        private void CreateStudentForm()
        {
            //populate drop down menu with student options
            comboBoxOptions.Items.Add(Options.Departments);
            comboBoxOptions.Items.Add(Options.Courses);
            comboBoxOptions.Items.Add(Options.Instructors);
            comboBoxOptions.Items.Add(Options.Registrations);

            //change form text
            Text = "UBC - Student";
        }

        /// <summary>
        /// Create new Instructor Form method.
        /// </summary>
        private void CreateInstructorForm()
        {
            //populate drop down menu with instructor options
            comboBoxOptions.Items.Add(Options.Departments);
            comboBoxOptions.Items.Add(Options.Courses);
            comboBoxOptions.Items.Add(Options.Instructors);
            comboBoxOptions.Items.Add(Options.Students);
            comboBoxOptions.Items.Add(Options.Registrations);

            //change form text
            Text = "UBC - Instructor";
        }

        /// <summary>
        /// Create new Admin Form method.
        /// </summary>
        private void CreateAdminForm()
        {
            //populate drop down menu with admin options
            comboBoxOptions.Items.Add(Options.Add_Delete_Course);
            comboBoxOptions.Items.Add(Options.Add_Delete_Section);

            //select add/delete course by default
            comboBoxOptions.SelectedItem = Options.Add_Delete_Course;

            //change form text
            Text = "UBC - Admin";
        }

        #endregion

        #region Form controls' event handlers

        /// <summary>
        /// Back button click event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonBack_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Go button click event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonGo_Click(object sender, EventArgs e)
        {
            //retrieve selected option from drop down menu
            Options option = (Options)Enum.Parse(typeof(Options), comboBoxOptions.SelectedItem.ToString());
            Hide(); //hide current form

            if(new UBCDataForm(option, user).ShowDialog() == DialogResult.OK)   //lock main thread on new form
                Show(); //unhide current form
        }

        /// <summary>
        /// Closing form event handler.
        /// </summary>
        /// <param name="sender">Object triggered event.</param>
        /// <param name="e">Event arguments.</param>
        private void UBCOptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonBack.PerformClick();  //fire back button click event
        }

        #endregion
    }
}