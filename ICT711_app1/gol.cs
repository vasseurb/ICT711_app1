/**********************************
 * Program: ICT711_app1
 * Author:  Brian Vasseur
 * Date:    June 29, 2019
 * Purpose: This program is designed to play the Game of Life simulation.
 *          
 *          BuildLifeMatrix - Load time operations to build a 30 x 30 matrix of cells
 *                              matrix size, cell size and position within the form are customizable
 *          ResetMatrix     - Reset the matrix to prepare for a file load or manual setting
 *          CopyLife_Matrix - Copy the current generation to a save array before calculating
 *          Matrix_Click    - Click on a cell to turn it on or off
 *          ReadFileAndLoadMatrix - open the file, read the input, populate the array and display the array
 *                          - called by the file load button
 *          
 *          The application allows for 1-9 generations when the calculate button is pushed and does error checking on the input
 *          There is a status bar that is displayed at the bottom for various actions such as file open/save and click
 *          
 *          The file input, safe and parsing routines are based on the examples used in class
 *          
 *          This solution is too reliant on global variables and could be optimized
 *          
 *  **********************************/

using System;
using System.IO; //file handler - not part of the default usings
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ICT711_app1
{
    public partial class gol : Form
    {
        //class variables declared here
        const int life_matrix_size  = 30; // assume a symmetrical matrix of cells
        const int life_cell_size    = 20; // cell size for the matrix of labels 
        const int life_cell_startxy =  0; // start position in the form

        // Two equal arrays, one for the current matrix and one for the previous life
        // add 2 extra cells so we have a valid way to check for neighbors
        bool[,] life_matrix = new bool[(life_matrix_size + 2), (life_matrix_size + 2)];
        bool[,] last_matrix = new bool[(life_matrix_size + 2), (life_matrix_size + 2)];

        int calc_generations; // how many generations should be processed in the next calculation;
        int show_generations; // what generation is currently being displayed

        public gol()
        {
            InitializeComponent();
        }

        // Form Load is a good place to create the matrix
        // Do a reset so we are starting with known values
        private void gol_Load(object sender, EventArgs e)
        {
            // Setup the matrix in the form on load
            BuildLifeMatrix();  // Build out a matrix of labels
            ResetMatrix();      // Start with a known state
        }

        // Create a matrix of labels on the form and name them by cell address
        // Called once
        // NOTE: Deviation from the usual solution!
        // The matrix is two cells wider and taller than the matrix used to process data
        // The first and last row and column will always be false, i.e. have no data
        // This is to simplify the neighbors calculation - it is not necessary to check if you
        // are at the start or end of a row / column and do special processing as there is always
        // a valid, but false, field bordering the matrix.
        //
        // implicit usage of System.Drawing for Color, Point, Size, etc.
        //
        // Disadvantage to doing this in Load is that you can't easily display a progress indicator

        private void BuildLifeMatrix()
        {
            int xpos;                       // x axis or column
            int ypos = life_cell_startxy;   // y axis or row
            string lbl_name = "X";          // the label name - initialize as X for error checking
            MessageBox.Show("Standby while the matrix is being created " +
                "\n\nThis will take a minute." +
                "\n\nPress Ok to Start...");
            for (int y = 0; y <= (life_matrix_size + 1); y++) // Y axis for row loop
            {
                xpos = life_cell_startxy; // set the X start to the beginning of the line
                for (int x = 0; x <= (life_matrix_size + 1); x++) // X axis for column loop
                {
                    lbl_name = "G" + y.ToString("D2") + x.ToString("D2");   // G + row + column
                    Label namelabel = new Label();
                    namelabel.Location = new Point(xpos, ypos);             //position the cell on the form
                    namelabel.Size = new Size(life_cell_size, life_cell_size); //set the size
                    namelabel.Name = lbl_name;                              //name the label according to location
                    namelabel.Tag = y * 100 + x;                            // set the tag to the same values
                    namelabel.BorderStyle = BorderStyle.None;               // start with no border
                    namelabel.BackColor = Color.Aqua;                       // default is Aqua fill
                    if (y > 0 & y <= life_matrix_size)
                    {
                        if (x > 0 & x <= life_matrix_size)                  // dont display the outside cells bordering the matrix
                        {
                            namelabel.BorderStyle = BorderStyle.FixedSingle; //give it a border
                            namelabel.BackColor = Color.White;               // part of the matrix - white bkgrnd
                        }
                    }
                    namelabel.Click += new System.EventHandler(matrix_Click); //plumb in a common event handler for Click
                    this.Controls.Add(namelabel); //create the control
                    xpos += life_cell_size; //increment the X axis position
                }
                ypos += life_cell_size; //move down the Y axis
            }
//            MessageBox.Show("Done building matrix");
        }

        // Clear all the values in the current and previous generation matrix
        // Could also do Array.Clear() but since we need to reset the matrix of labels
        // a for loop is required anyway.
        private void ResetMatrix()
        {
            // string name = "the_name_you_know";
            // Control ctn = this.Controls[name];
            // ctn.Text = "Example...";

            // The matrix of labels is in the format Gxxyy where xx and yy are the coordinates
            // First reset the arrays, then reset the background colors back to transparent
            // Ignore the first and last array values, this represents the border
            string lbl_name = "x"; // we need a place to create a label name
            Control lbl_ctl = this.Controls[lbl_name]; // create a control to reference the label 

            for (int y = 1; y <= (life_matrix_size); y++) // Y axis for loop
            {
                for (int x = 1; x <= (life_matrix_size); x++) // X axis for loop
                {
                    life_matrix[y, x] = false; // blanks the current matrix
                    last_matrix[y, x] = false; // blanks the previous generation
                    lbl_name = "G" + y.ToString("D2") + x.ToString("D2"); // format is Gxxyy i.e G1324
                    lbl_ctl = this.Controls[lbl_name]; // specify what label we are referring to
                    lbl_ctl.BackColor = System.Drawing.Color.White;
                }
            }
            btn_calc.Enabled = true; // enable the Calc button if we disabled it in the last round
            calc_generations = 1;    // Default to one generation
            show_generations = 1;    // First generation
            txt_gen_calc.Text = calc_generations.ToString();
            txt_gen_current.Text = show_generations.ToString(); //update the form data
            txt_statusbar.Text = "Load a data file or select fields"; // Reset the status bar too.
        }

        // Copy the current life_matrix to last_matrix
        // This way we can make changes to life matrix while calculating values but use
        // last matrix to keep track of neighbors from the previous generation.
        // can't do last_matrix = life_matrix as this just points both arrays to the same heap
        private void CopyLifeMatrix()
        {
            int i, j;
            for (i=0; i < (life_matrix_size+1); i++) 
            {
                for (j = 0; j < (life_matrix_size+1); j++)
                {
                    last_matrix[i, j] = life_matrix[i, j]; // cell by cell copy of each array value
                }
            }
        }

        // This is a common event handler for the label cells in the matrix
        // Clicking on a matrix field toggles the life status of that cell
        // A cell is alive if the background is green, dead if it is white.
        // A blue background is a border cell which is also set false to represent no neighbor
        private void matrix_Click(object sender, EventArgs e)
        {
            Label matrix_cell = (Label)sender;
            int col = (int)matrix_cell.Tag / 100;    // row value
            int row = (int)matrix_cell.Tag % 100;    // column value

            if (matrix_cell.BackColor == Color.Aqua) // this is a border field so do nothing
                return;
            else if (matrix_cell.BackColor == Color.Lime) // could also check life_matrix boolean array
            {
                matrix_cell.BackColor = Color.White; // display a white background
                life_matrix[col, row] = false;       // set the cell value to dead
                txt_statusbar.Text = "Cell " + matrix_cell.Name + " is Dead";
            }
            else
            {
                matrix_cell.BackColor = Color.Lime; // display a green background
                life_matrix[col, row] = true;       // set the cell value to alive
                txt_statusbar.Text = "Cell " + matrix_cell.Name + " is Alive";
            }
        }


        // Parsing routine to read the input file for a pair of values and populate the Matrix
        // Parsing is used almost exactly from the course sample but with no output parameters
        // 
        private bool readFileAndLoadMatrix(StreamReader file)
        {
            string   lineOfInput;                      // string to get a line from the file
            string[] numbers;                          // string array that gets the Split() of a given line from file
            char[]   delimiters = { ' ', ',', '\t' };  // delimiters used to split a line from file into tokens
            int      value;                            // place to stuff value from a TryParse() of a token
            int      num_found;                        // need to identify the first and second values
            int      row_index;
            int      col_index;                        // Store the row and column for each matrix cell.             

            // while there is still text in the file ...
            //      1) read a line of text, triming leading and trailing blanks
            //      2) split the line into tokens based on the delimiters character array
            //      3) for each token in a line, try to convert to an integer. If fails, output message and give up.
            //      4) if successfull, increment the num_found count and set the array value.

            while (!file.EndOfStream)
            {
                try      // use a try catch block to catch any IO errors in processing
                {
                    lineOfInput = file.ReadLine().Trim();
                    if (lineOfInput.Length != 0)
                    {
                        // split the line into individual tokens and then process each token in turn
                        // we are only interested in the first two numbers
                        // this routine would be better if it did more error checking to make sure that 
                        // there was at least two numbers, and pop up an error message if more than 2 per record
                        // 
                        num_found = 0;  // need to keep track of the row index and column index inputs
                        row_index = 0;  // clear for each record
                        col_index = 0;

                        numbers = lineOfInput.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string num in numbers)
                        {
                            // convert the token to an int - if fails, give up
                            if (!int.TryParse(num, out value))
                            {
                                MessageBox.Show("Value in file contains non numeric data.");
                                return false;
                            }

                            num_found++;                // increment the counter of numbers found in this record
                            if (num_found == 1)
                                row_index = value+1;    // add 1 to the value from the input file because our matrix starts at 1
                            if (num_found == 2)
                                col_index = value+1;    // same for the column - position 0 is a border cell

                        }
                        // OK we are done reading the record
                        // we should have a row column pair, populate the matrix
                        // set the life_matrix cell to true
                        // set the cell background to lime;

                        Control matrix_cell = Controls["G" + row_index.ToString("D2") + col_index.ToString("D2")]; //access the control by it's name
                        life_matrix[row_index,col_index] = true;               // cell value is alive
                        matrix_cell.BackColor = Color.Lime;                    // display a green background

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"File read error: {e.Message}.");
                    return false;
                }
            }

            // managed to read the file without error, so just return with a true value

            return true;
        }

        // Button to load a file, read input and build the matrix
        // This button creates a file dialog giving the user the option to select an input file
        // Once a file is selected it tries to open the file, and if that is successful calls a method to build the Matrix
        // If no file is selected then just return back to the form
        // This routine borrows heavily from the sample code provided in class
        private void btn_load_Click(object sender, EventArgs e)
        {
            // Method to load a GOL file and build out the matrix
            // The Matrix is created at load time, just populate the array and display
            //
            // Parsing code used from sample files
            //
            StreamReader file;                         // reference to the StreamReader() object used to access the input file
            string   lineOfInput;                      // string to get a line from the file
            string[] numbers;                          // string array that gets the Split() of a given line from file
            char[]   delimiters = { ' ', ',', '\t' };  // delimiters used to split a line from file into tokens
            int      value;                            // place to stuff value from a TryParse() of a token
        
            ResetMatrix();                             // clear the arrays so we can start a new model
            txt_statusbar.Text = "";                   // clear the status bar

            OpenFileDialog dialog = new OpenFileDialog();               // reference to the open file dialog
            dialog.Title = "Open Data File";                            // title for the File dialog
            dialog.Filter = "All Files|*.*|Gol Files (*.gol)|*.gol";    // all files or .gol files in picklist
            dialog.FilterIndex = 2;                                     // default to second option

            if (!(dialog.ShowDialog() == DialogResult.OK))                 // nothing selected
                return;
            else
            {
                txt_statusbar.Text = dialog.FileName;                   // display the filename in the status field
            }
            // OK got this far so we have a file selected
            // try to open the file. If fails then tell use via a MessageBox() and give up
            try
            {
                file = new StreamReader(txt_statusbar.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File did not open: {ex.Message}.");
                return;
            }

            // if file sucessfully opened then pass the StreamReader object to the readFileAndLoadMatrix() method
            // read the file input and build the matrix.
            // Only check for error, if there is a successful return the matrix loaded correctly.

            if (!readFileAndLoadMatrix(file))
                MessageBox.Show("Bad Matrix");  // Method completed with error, likely a bad input file

            file.Close();   // We are done reading input, close the file before we leave.
        }


        // Save results button. Provide a file save dialog and then call a routine
        // to write out the results of the array
        private void btn_save_Click(object sender, EventArgs e)
        {
            // Method to load a GOL file and build out the matrix
            // Form is created at load time, just populate the array and display
            StreamWriter file;      // reference to the StreamWriter() object used to connect to the ouptut file
            int row, col;           // for loop counters as we process the matrix

            txt_statusbar.Text = "";  // clear the status bar before we pick a file
            SaveFileDialog dialog = new SaveFileDialog();    // reference to the Save file dialog
            dialog.Title = "Save Data File";
            dialog.Filter = "Gol Files (*.gol)|*.gol"; //suggest what extension is being used (display only!)
            dialog.FilterIndex = 1;
            dialog.DefaultExt = ".gol";                // this is what really sets the default extension

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txt_statusbar.Text = dialog.FileName; // display the filename in the status field
            }

            // OK if we have a valid file then write out the array results
            try
            {
                file = new StreamWriter(txt_statusbar.Text);
                for (row=1; row <= life_matrix_size; row++)         // loop thru all the rows
                {
                    for (col=1; col <= life_matrix_size; col++)     // loop thru all the columns
                    {
                        if (life_matrix[col,row])                   // found a life cell write the output
                            file.WriteLine((col-1).ToString() + " " + (row-1).ToString()); // subtract 1 from the values because of border
                    }
                }
                file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File not created. {ex.Message}");
            }
        }

        private void btn_calc_Click(object sender, EventArgs e)
        {
            // calculate a generation
            // Ignore the border so we are checking cells 1 thru 30 (life_matrix_size)
            // Logic is to check vertical, horizontal and diagonal so +/- 1 but not myself!
            // alive_count < 2       - cell dies
            // alive_count >=2 & <=3 - cell lives
            // alive_count >3        - cell dies

            int alive_count;     // how many neighbors are alive
            bool stable_matrix;  // check to see if this generation is different than the last
            int x, y, g, nx, ny; // loop counters position, generation and neighbor xy, 

            // loops go here
            // add 1 to show_generations each time through the loop
            // check every iteration that life and last matrix are not the same - then stop
            for (g = 1; g <= calc_generations; g++) // repeat for as many generations as necessary
            {
                stable_matrix = true;       // new generation so assume it will match the last until we find a change
                CopyLifeMatrix();           // save the current generation before we start processing;

                for (y = 1; y <= (life_matrix_size); y++)       // Y axis for loop
                {
                    for (x = 1; x <= (life_matrix_size); x++)   // X axis for loop
                    {
                        alive_count = 0;                        // new cell so prepare to check the neighbors
                        for (nx = -1; nx <= 1; nx++)            // calculate x position of the neighbors
                        {
                            for (ny = -1; ny <= 1; ny++)        // calculate y position of the neighbors
                            {
                                if (last_matrix[(x + nx), (y + ny)])    // this neighbor cell is alive
                                {                                       // check the last matrix as life matrix is changing
                                    if (nx != 0)                        // not in my column, it's not me
                                        alive_count++;
                                    else
                                    {
                                        if (ny != 0)              // not in my row either, it's not me
                                            alive_count++;        // add me to alive neighbors
                                    }
                                }
                            }
                        }

                        Control matrix_cell = Controls["G" + x.ToString("D2") + y.ToString("D2")]; //access the control by it's name

                        if (alive_count >= 2 & alive_count <= 3)    // we've checked all the neighbors
                        {
                            life_matrix[x, y] = true;               // we live this round
                            matrix_cell.BackColor = Color.Lime;     // display a green background
                        }
                        else
                        {
                            life_matrix[x, y] = false;              // we die this round
                            matrix_cell.BackColor = Color.White;    // display a white background
                        }

                        if (!last_matrix[x, y] == life_matrix[x, y])    // this cell had a status change
                        {
                            stable_matrix = false;                  // something changed in this generation
                        }
                    }
                }
                // Generations Loop
                if (stable_matrix)
                {
                    g = calc_generations + 1;               // no more generations
                    btn_calc.Enabled = false;               // disable the Calc button
                }
            }

            // reset calc_generations back to 1
            // Update the form with current data before we leave
            calc_generations = 1;
            txt_gen_calc.Text = calc_generations.ToString();
            txt_gen_current.Text = show_generations.ToString(); //update the form data
            txt_statusbar.Text = "Ready to calculate";
        }


        //Reset the matrix back to initial settings and clear the status bar
        private void btn_reset_Click(object sender, EventArgs e)
        {
            ResetMatrix(); // Start with a known state
        }

        // Exit Buton
        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Terminate and close the app
        }

        // Method to check that a user didn't leave a textbox empty.
        private bool TextNotEmpty(string input_value)
        {
            // Check for Null, no entry or just spaces. All are invalid
            if (string.IsNullOrWhiteSpace(input_value))
            {
                MessageBox.Show("This is a required field, please try again");
                return false;
            }
            else
                return true;
        }

        private bool ValidIntegerInput(double input_number, double low_limit, double high_limit)
        {

            if (input_number < low_limit | input_number > high_limit)
            {
                MessageBox.Show("Input must be between " +
                    low_limit.ToString("f0") +
                    " and " +
                    high_limit.ToString("f0"));
                return false;
            }
            return true;
        }


        private void txt_gen_calc_Validating(object sender, CancelEventArgs e)
        {
            // validate for integer values from 1-9
            bool valid_generations = true; // assume user entered valid input to start

            // Check that something was entered, don't check for numbers yet
            if (!TextNotEmpty(txt_gen_calc.Text))
            {
                valid_generations = false; // flag that validation has failed
            }
            else
            {
                // try converting the input text to int32
                // if that works then also check that it is within a valid range
                // we've already checked that it is not blank
                try
                {
                    calc_generations = Convert.ToInt32(txt_gen_calc.Text); // try converting to Int32

                    // valid choices are 1-9
                    if (!ValidIntegerInput(calc_generations, 1, 9))  // check if the number is in range
                    {
                        valid_generations = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter a number"); // try statement failed
                    valid_generations = false; // failed on try so fail validation
                }

            }

            if (!valid_generations)
            {
                txt_gen_calc.Clear();
                txt_gen_calc.Focus(); //clear the input and go back to field
            }

        }

    }
}
