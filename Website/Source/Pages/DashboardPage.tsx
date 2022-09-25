import React, { MouseEvent, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { getApiBaseUrl } from '../Utils';
import { UserProfileResponse } from '../Models/UserProfileResponse';
import FileSaver from 'file-saver';
import { clearSession, getSession } from '../Utils/AuthUtils';
import { useTranslation } from 'react-i18next';
import { UpdateUserProfileRequest } from '../Models/UpdateUserProfileRequest';
import copy from 'copy-to-clipboard';

export const DashboardPage: React.FunctionComponent<any> = () => {
  const history = useHistory();
  const [isLoading, setLoading] = useState(false);
  const [isDisabled, setDisabled] = useState(false);
  const [isSettingDisabledPreference, setSettingDisabledPreference] = useState(false);
  const [configContents, setConfigContents] = useState<string | null>(null);

  const { t } = useTranslation();

  useEffect(() => {
    const sessionId = getSession();

    if (sessionId === null) {
      clearSession();
      history.push('/login');
    }

    loadProfile();
  }, []);

  const loadProfile = (): void => {
    void (async function () {
      try {
        const sessionId = getSession();

        if (sessionId === null) {
          return;
        }

        const requestOptions = {
          method: 'GET',
          headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${sessionId}` }
        };

        setLoading(true);
        const response = await fetch(getApiBaseUrl() + '/user-profile', requestOptions);

        if (response.status >= 400) {
          console.log('Oh, no!');
        }

        const userProfileResponse: UserProfileResponse = await response.json();
        setDisabled(userProfileResponse.isDisabled);

        const encodedSpotifyUserId = encodeURIComponent(userProfileResponse.spotifyUserID);
        const encodedCFGKey = encodeURIComponent(userProfileResponse.cfgKey);

        const userConfig = `"CS:GO Tunes"
{
 "uri" "${getApiBaseUrl()}/game-state?spotifyUserID=${encodedSpotifyUserId}&cfgKey=${encodedCFGKey}"
 "timeout" "5.0"
 "buffer"  "0.1"
 "throttle" "0.5"
 "heartbeat" "60.0"
 "output"
 {
   "precision_time" "3"
   "precision_position" "1"
   "precision_vector" "3"
 }
 "data"
 {
   "provider"            "1"      // general info about client being listened to: game name, appid, client steamid, etc.
   "map"                 "1"      // map, gamemode, and current match phase ('warmup', 'intermission', 'gameover', 'live') and current score
   "round"               "1"      // round phase ('freezetime', 'over', 'live'), bomb state ('planted', 'exploded', 'defused'), and round winner (if any)
   "player_id"           "1"      // player name, clan tag, observer slot (ie key to press to observe this player) and team
   "player_state"        "1"      // player state for this current round such as health, armor, kills this round, etc.
   "player_weapons"      "1"      // output equipped weapons.
   "player_match_stats"  "1"      // player stats this match such as kill, assists, score, deaths and MVPs
 }
}`;

        setConfigContents(userConfig);
      } finally {
        setLoading(false);
      }
    })();
  };

  const setDisabledPreference = (isDisabled: boolean): void => {
    void (async function () {
      try {
        const sessionId = getSession();

        if (sessionId === null) {
          return;
        }

        const request: UpdateUserProfileRequest = {
          isDisabled
        };

        const requestOptions: RequestInit = {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${sessionId}`
          },
          body: JSON.stringify(request)
        };

        setSettingDisabledPreference(true);
        const response = await fetch(getApiBaseUrl() + '/user-profile', requestOptions);

        if (response.status >= 400) {
          console.log('Oh, no!');
        }

        loadProfile();
      } finally {
        setSettingDisabledPreference(false);
      }
    })();
  };

  const downloadConfig = (e: MouseEvent<HTMLButtonElement>): void => {
    if (configContents === null) {
      return;
    }

    const blob = new Blob([configContents], {
      type: 'text/plain;charset=utf-8'
    });
    FileSaver.saveAs(blob, 'gamestate_integration_csgotunes.cfg');
  };

  const copyConfig = (e: MouseEvent<HTMLButtonElement>): void => {
    if (configContents === null) {
      return;
    }

    copy(configContents);
  };
  return (
    <div>
      <p>{t('dashboard_welcome')}</p>
      <div>
        <button onClick={downloadConfig} disabled={isLoading}>{t('download_cfg_button')}</button>
      </div>
      <div>
      {isDisabled
        ? (
            <button onClick={(e: MouseEvent<HTMLButtonElement>) => setDisabledPreference(false)} disabled={isLoading}>{t('enable_button')}</button>
          )
        : (
        <button onClick={(e: MouseEvent<HTMLButtonElement>) => setDisabledPreference(true)} disabled={isLoading || isSettingDisabledPreference}>{t('disable_button')}</button>
          )}
      </div>
      <div>
        <textarea value={configContents === null ? '' : configContents} readOnly={true} />
      </div>
      <div>
        <button onClick={copyConfig} disabled={isLoading}>{t('copy_cfg_to_clipboard_button')}</button>
      </div>
    </div>
  );
};
