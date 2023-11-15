import React, { useState, useEffect, useRef } from 'react'
import {
  View,
  ImageBackground,
  Platform,
  Switch,
  ActivityIndicator,
  TouchableOpacity,
  Linking,
  AppState
} from 'react-native'
import TextDefault from '../Text/TextDefault/TextDefault'
import { colors } from '../../utilities/colors'
import bg from '../../assets/restBackground.png'
import styles from './styles'
import { Icon } from 'react-native-elements/dist/icons/Icon'
import { useAccount } from '../../ui/hooks'
import { Image } from 'react-native-elements'
import useNotification from '../../ui/hooks/useNotification'
import { PRODUCT_URL, PRIVACY_URL, ABOUT_URL } from '../../utilities'

export default function SideBar() {
  const notificationRef = useRef(true)
  const openSettingsRef = useRef(false)
  const { logout, data, toggleSwitch, isAvailable } = useAccount()
  const [notificationStatus, setNotificationStatus] = useState(false)
  const appState = useRef(AppState.currentState)

  const {
    restaurantData,
    getPermission,
    getExpoPushToken,
    requestPermission,
    sendTokenToBackend
  } = useNotification()

  useEffect(() => {
    const checkToken = async () => {
      if (restaurantData) {
        setNotificationStatus(restaurantData.restaurant.enableNotification)
        if (
          restaurantData.restaurant.enableNotification &&
          notificationRef.current
        ) {
          const permissionStatus = await getPermission()
          if (permissionStatus.granted) {
            setNotificationStatus(true)
            const token = (await getExpoPushToken()).data
            sendTokenToBackend({ variables: { token, isEnabled: true } })
          }
        }
        notificationRef.current = false
      }
    }
    checkToken()
  }, [restaurantData])

  useEffect(() => {
    const subscription = AppState.addEventListener('change', nextAppState => {
      if (
        appState.current.match(/inactive|background/) &&
        nextAppState === 'active'
      ) {
        const checkTokenOnFocus = async () => {
          if (
            !notificationStatus &&
            (await getPermission()).granted &&
            openSettingsRef.current
          ) {
            setNotificationStatus(true)
            const token = (await getExpoPushToken()).data
            sendTokenToBackend({ variables: { token, isEnabled: true } })
          }
        }
        if (!notificationRef.current) checkTokenOnFocus()
      }

      appState.current = nextAppState
    })
    return () => {
      subscription && subscription.remove()
    }
  }, [])

  const handleClick = async () => {
    if (notificationStatus === false) {
      const permissionStatus = await getPermission()
      if (permissionStatus.granted) {
        setNotificationStatus(true)
        const token = (await getExpoPushToken()).data
        sendTokenToBackend({ variables: { token, isEnabled: true } })
      } else if (permissionStatus.canAskAgain) {
        const result = await requestPermission()
        if (result.granted) {
          setNotificationStatus(true)
          const token = (await getExpoPushToken()).data
          sendTokenToBackend({ variables: { token, isEnabled: true } })
        }
      } else {
        openSettingsRef.current = true
        Platform.OS === 'ios'
          ? Linking.openURL('app-settings:')
          : Linking.openSettings()
      }
    } else {
      setNotificationStatus(false)
      sendTokenToBackend({ variables: { token: null, isEnabled: false } })
    }
  }
  return (
    <View style={{ flex: 1 }}>
      <ImageBackground source={bg} resizeMode="cover" style={styles.image}>
        <View style={styles.topContainer}>
          <View style={styles.profileContainer}>
            <View style={styles.avatar}>
              <Image
                source={{ uri: data && data.restaurant.image }}
                containerStyle={styles.item}
                style={{ borderRadius: 5 }}
                PlaceholderContent={<ActivityIndicator />}
              />
            </View>
            <View style={{ width: '50%' }}>
              <TextDefault
                H5
                bolder
                center
                textColor="white"
                style={{ marginLeft: 10, textAlign: 'left' }}>
                {data && data.restaurant.name}
              </TextDefault>
            </View>
          </View>
        </View>
        <View style={styles.middleContainer}>
          <View style={styles.status}>
            <TextDefault H4 bolder textColor="white">
              Status
            </TextDefault>
            <View style={{ flexDirection: 'row', alignItems: 'center' }}>
              <TextDefault textColor="white" style={{ marginRight: 5 }}>
                {isAvailable ? 'Online' : 'Closed'}
              </TextDefault>
              <Switch
                trackColor={{
                  false: colors.fontSecondColor,
                  true: colors.green
                }}
                thumbColor={isAvailable ? colors.headerBackground : '#f4f3f4'}
                ios_backgroundColor="#3e3e3e"
                onValueChange={toggleSwitch}
                value={isAvailable}
                style={{ marginTop: Platform.OS === 'android' ? -15 : -5 }}
              />
            </View>
          </View>
          <View style={styles.status}>
            <TextDefault H4 bolder textColor="white">
              Notifications
            </TextDefault>
            <View style={{ flexDirection: 'row', alignItems: 'center' }}>
              <TextDefault textColor="white" style={{ marginRight: 5 }}>
                {notificationStatus ? 'On' : 'Off'}
              </TextDefault>
              <Switch
                trackColor={{
                  false: colors.fontSecondColor,
                  true: colors.green
                }}
                thumbColor={
                  notificationStatus ? colors.headerBackground : '#f4f3f4'
                }
                ios_backgroundColor="#3e3e3e"
                onValueChange={handleClick}
                value={notificationStatus}
                style={{ marginTop: Platform.OS === 'android' ? -15 : -5 }}
              />
            </View>
          </View>
          <TouchableOpacity
            style={styles.logout}
            activeOpacity={0.8}
            onPress={() =>
              Linking.canOpenURL(PRODUCT_URL).then(() => {
                Linking.openURL(PRODUCT_URL)
              })
            }>
            <View style={styles.icon}>
              <Icon
                type="font-awesome"
                color="white"
                name="product-hunt"
                size={26}
              />
            </View>
            <TextDefault H4 bolder style={styles.text}>
              Product page
            </TextDefault>
          </TouchableOpacity>
          <TouchableOpacity
            style={styles.logout}
            activeOpacity={0.8}
            onPress={() =>
              Linking.canOpenURL(
                'https://multivendor.enatega.com/?_gl=1*gjjx59*_ga*NTczMDY0NDU1LjE2ODUzMzgzODg.*_ga_DTSL4MVB5L*MTY5NjQ4MDQ3Ni41NC4xLjE2OTY0ODA0ODMuNTMuMC4w&_ga=2.17598781.520939582.1696480479-573064455.1685338388#/privacy'
              ).then(() => {
                Linking.openURL(
                  'https://multivendor.enatega.com/?_gl=1*gjjx59*_ga*NTczMDY0NDU1LjE2ODUzMzgzODg.*_ga_DTSL4MVB5L*MTY5NjQ4MDQ3Ni41NC4xLjE2OTY0ODA0ODMuNTMuMC4w&_ga=2.17598781.520939582.1696480479-573064455.1685338388#/privacy'
                )
              })
            }>
            <View style={styles.icon}>
              <Icon type="font-awesome" color="white" name="lock" size={26} />
            </View>

            <TextDefault H4 bolder style={styles.text}>
              Privacy policy
            </TextDefault>
          </TouchableOpacity>
          <TouchableOpacity
            style={styles.logout}
            activeOpacity={0.8}
            onPress={() =>
              Linking.canOpenURL(ABOUT_URL).then(() => {
                Linking.openURL(ABOUT_URL)
              })
            }>
            <View style={styles.icon}>
              <Icon
                type="font-awesome"
                color="white"
                name="info-circle"
                size={26}
              />
            </View>
            <TextDefault H4 bolder style={styles.text}>
              About us
            </TextDefault>
          </TouchableOpacity>
        </View>
        <View style={styles.lowerContainer}>
          <TouchableOpacity style={styles.logout} onPress={logout}>
            <View style={styles.icon}>
              <Icon type="entypo" color="white" name="log-out" size={26} />
            </View>
            <TextDefault H4 bolder style={styles.text}>
              Logout
            </TextDefault>
          </TouchableOpacity>
        </View>
      </ImageBackground>
    </View>
  )
}
