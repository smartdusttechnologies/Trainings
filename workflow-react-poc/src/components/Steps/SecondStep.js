import React from "react";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import FormControlLabel from "@mui/material/FormControlLabel";
import Checkbox from "@mui/material/Checkbox";
import { FormControl, Grid, InputLabel, MenuItem, Select } from "@mui/material";

const SecondStep = () => {
  return (
    <div>
      <Typography variant="h6" gutterBottom>
        Payment details
      </Typography>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <FormControl fullWidth>
            <InputLabel id="demo-select-small-label">Payment Method</InputLabel>
            <Select required id="method" name="method" label="Payment Method">
              <MenuItem value={"credit"}>Credit Card</MenuItem>
              <MenuItem value={"debit"}>Debit Card</MenuItem>
              <MenuItem value={"upi"}>UPI</MenuItem>
            </Select>
          </FormControl>
        </Grid>
        <Grid item xs={12}>
          <TextField
            required
            id="price"
            name="price"
            label="Price"
            fullWidth
            autoComplete="price"
          />
        </Grid>
        <Grid item xs={12}>
          <FormControlLabel
            control={<Checkbox color="secondary" name="Checkbox" value="yes" />}
            label="Checkbox"
          />
        </Grid>
      </Grid>
    </div>
  );
};

export default SecondStep;
