import {
  Box,
  Button,
  ButtonBase,
  CircularProgress,
  Container,
  Paper,
  Radio,
  Typography,
  useTheme,
} from "@mui/material";
import React from "react";
import CodIcon from "../../assets/icons/CodIcon";
import MastercardIcon from "../../assets/icons/MastercardIcon";
import PayPalIcon from "../../assets/icons/PayPalIcon";
import VisaIcon from "../../assets/icons/VisaIcon";
// import useStyles from "./styles";
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import { useNavigate } from "react-router-dom";

const PAYMENT_OPTIONS = [
  {
    id: 0,
    payment: "STRIPE",
    label: "Credit / Debit Card",
    icon: <VisaIcon />,
    icon1: <MastercardIcon />,
  },
  {
    id: 1,
    payment: "PAYPAL",
    label: "Paypal",
    icon: <PayPalIcon />,
  },
  {
    id: 2,
    payment: "COD",
    label: "Cash",
    icon: <CodIcon />,
  },
];

function PaymentCard({
  paymentMethod,
  setPaymentMethod,
  validateOrder,
  onPayment,
  loading,
}) {
  const navigate = useNavigate();

  const theme = createTheme();
  // const classes = useStyles();

  return (
    <ThemeProvider theme={theme}>
      <Paper
        style={{
          background: theme.palette.common.white,
          borderRadius: "inherit",
          paddingBottom: theme.spacing(2),
          paddingTop: theme.spacing(2),
          marginTop: theme.spacing(4),
          margin:'auto',
        }}
      >
        <Container>
          <Box display="flex" alignItems="center">
            <Box ml={theme.spacing(1)} />
            <Typography variant="h5" color="textSecondary">
              Payment
            </Typography>
          </Box>
          {PAYMENT_OPTIONS.map((item, index) => (
            <ButtonBase
              key={`CARD_${index}`}
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "space-between",
                width: "100%",
                height: "100%",
                padding: theme.spacing(1),
                marginTop: theme.spacing(4),
                border: `1px solid ${theme.palette.grey[300]}`,
              }}
              onClick={() => setPaymentMethod(item)}
            >
              <Box display="flex" alignItems="center">
                <Radio
                  color="primary"
                  // checked={paymentMethod.id === item.id}
                  onChange={() => setPaymentMethod(item)}
                />
                <Typography variant="body1" color="textSecondary">
                  {item.label}
                </Typography>
              </Box>
              <Box display="flex">
                {item.icon}
                {item.icon1 && (
                  <>
                    <Box ml={theme.spacing(1)} />
                    {item.icon1}
                  </>
                )}
              </Box>
            </ButtonBase>
          ))}
          <Button
            disabled={loading}
            style={{
              maxWidth: "auto",
              padding: `${theme.spacing(2)} 0`,
              background: theme.palette.primary.main,
              marginTop: theme.spacing(2),
              width: "100%",
              borderRadius: 0,
            }}
            onClick={() => {
              navigate('/success')
              // if (validateOrder()) onPayment();
            }}
          >
            {loading ? (
              <CircularProgress color="secondary" size={20} />
            ) : (
              <Typography
                style={{
                  ...theme.typography.body1,
                  color: theme.palette.common.white,
                  fontWeight: 700,
                }}
              >
                PLACE ORDER
              </Typography>
            )}
          </Button>
          <Box mt={theme.spacing(2)} />
          <Typography
            variant="caption"
            style={{
              color: theme.palette.text.disabled,
            }}
          >
            I agree and I demand that you execute the ordered service before the
            end of the revocation period. I am aware that after complete
            fulfillment of the service I lose my right of recission. Payment
            transactions will be processed abroad.
          </Typography>
        </Container>
      </Paper>
    </ThemeProvider>
  );
}

export default PaymentCard;
