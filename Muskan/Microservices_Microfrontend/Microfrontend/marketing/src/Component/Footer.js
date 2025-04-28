import React from "react";
import { Container, Grid, Typography, Link, Box } from "@mui/material";
import Copyright from "./Copyright";

// Sample data structure for footers
const footers = [
  {
    title: "Company",
    description: ["About Us", "Careers", "Contact Us"],
  },
  {
    title: "Features",
    description: ["Cool stuff", "Random feature", "Team feature"],
  },
  {
    title: "Resources",
    description: ["Resource", "Resource name", "Another resource"],
  },
  {
    title: "Legal",
    description: ["Privacy policy", "Terms of use"],
  },
];

const Footer = () => {
  return (
    <Container
      maxWidth="md"
      component="footer"
      sx={{
        borderTop: 1,
        borderColor: "divider",
        mt: 8,
        py: { xs: 3, sm: 6 },
      }}
    >
      <Grid container spacing={4} justifyContent="space-evenly">
        {footers.map((footer) => (
          <Grid item xs={6} sm={3} key={footer.title}>
            <Typography variant="h6" color="text.primary" gutterBottom>
              {footer.title}
            </Typography>
            <ul style={{ listStyle: "none", padding: 0, margin: 0 }}>
              {footer.description.map((item) => (
                <li key={item}>
                  <Link href="#" variant="subtitle1" color="text.secondary">
                    {item}
                  </Link>
                </li>
              ))}
            </ul>
          </Grid>
        ))}
      </Grid>
      <Box mt={5}>
        <Copyright />
      </Box>
    </Container>
  );
};

export default Footer;
