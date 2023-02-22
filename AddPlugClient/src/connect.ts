async function onSubmit() {
  let ssid = (document.getElementById("ssid") as HTMLInputElement).value;
  let pass = (document.getElementById("pw") as HTMLInputElement).value;

  await fetch("http://192.168.33.1/settings?allow_cross_origin=1"); // Enable CORS
  await fetch(`http://192.168.33.1/settings?mqtt_enable=true
      &mqtt_user=admin&mqtt_pass=password&mqtt_server=192.168.227.65:1883`); // Enable and configure MQTT
  await fetch(`http://192.168.33.1/settings?coiot_enable=0`); // Disable CoIot

  fetch(`http://192.168.33.1/reboot`); // Reboot device

  setTimeout(
    () =>
      fetch(`http://192.168.33.1/settings/sta?enabled=1&ssid=${ssid}&key=${pass}`).then(() => {
        document.getElementById("working")!.hidden = true;
        document.getElementById("success")!.hidden = false;
      }),
    2000
  ); // Configure WiFi
}

function togglePassword() {
  var x = document.getElementById("pw") as HTMLInputElement;
  if (x.type === "password") {
    x.type = "text";
  } else {
    x.type = "password";
  }
}
