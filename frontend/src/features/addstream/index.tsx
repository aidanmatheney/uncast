import React, { Component, FunctionComponent } from 'react';
import styled, { css, ThemeProvider } from 'styled-components';
import axios from 'axios';

import UserTabId from '../../common/UserTabId';
import AddStreamTabId from '../../common/AddStreamTabId';

import {
  FaPlus,
  FaFileUpload,
  FaRss,
  FaYoutube
} from 'react-icons/fa';
import { IconType } from 'react-icons/lib'
import '../../index.css';
import { EventEmitter } from 'events';

const MenuContainer = styled.div`
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  width: 100%;
  background: ${props => props.theme.background};
`;

const MenuButton = styled.button`
  text-align: center;
  margin: 0.25rem;
  cursor: pointer;  
  color: ${props => props.theme.color};
  font-size: 1em;
  width: 525px;
  background: ${props => props.theme.background};
`; 

//styled-components themes
const ThemeStandard = {
  background: "#8c7ce2",
  color: "#ffffff",
  input: "000000"
};
const ThemeStandardBG = {
  background: "#3b1570"
};
const ThemeLight = {
  background: "#ecccff",
  color: "#000000"
};
const ThemeLightBG = {
  background: "#b8abc9"
};


const FileTab: {
  tab: AddStreamTabId;
  name: string;
  Icon: IconType;
}[] = [
  {
    tab: AddStreamTabId.AddFile,
    name: 'Add from file',
    Icon: FaFileUpload
  }
];

const RSSTab: {
  tab: AddStreamTabId;
  name: string;
  Icon: IconType;
}[] = [
  {
    tab: AddStreamTabId.AddRss,
    name: 'Add from RSS',
    Icon: FaRss
  }
];

const YouTubeTab: {
  tab: AddStreamTabId;
  name: string;
  Icon: IconType;
}[] = [
  {
    tab: AddStreamTabId.AddYouTube,
    name: 'Add from YouTube',
    Icon: FaYoutube
  }
];


const IconContainer = styled.div``;
const TextContainer = styled.div``;

const TabDescriptions: {
  tab: UserTabId;
  name: string;
  Icon: IconType;

}[] = [
  {
    tab: UserTabId.AddAudio,
    name: 'Add Audio',
    Icon: FaPlus
  }
];

//-----------------------------------------------------------------------------------------
//for getting file input
class FileForm extends React.Component <{}, { value: string; selectedFile: any; loaded: any}> {
  constructor(props: any) {
    super(props);
    this.state = {
      value: '',
      selectedFile: null,
      loaded: 0,
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }
  
  handleChange(event: any) {
    this.setState({
      value: event.target.value,
      selectedFile: event.target.files[0],
      loaded: 0,
    });
    console.log(event.target.files[0]);
  }

  handleSubmit(event: any) {
    //Add file
    const data = new FormData()
    data.append('file',this.state.selectedFile)
    axios.post("http://localhost:3306/upload", data, {
    })
    .then(res => {
      console.log(res.statusText)
    })
    

    event.preventDefault();
  }

  render() {
    return (
      <ThemeProvider theme={ThemeStandard}>
      <form onSubmit={this.handleSubmit}>
        <label>
          Audio File:
          <input type="file" value={this.state.value} onChange={this.handleChange} />
        </label>
        <input type="submit" value="Submit" />
      </form>
      </ThemeProvider>
    );
  }
}
export { FileForm };
//-----------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
//for getting RSS input
class RSSForm extends React.Component <{}, { value: string; selectedRSS: any;}> {
  constructor(props: any) {
    super(props);
    this.state = {
      value: '',
      selectedRSS: '',
  };
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(event: any) {
    this.setState({
      value: event.target.value,
      selectedRSS: event.target.value,
    });
    
  }

  handleSubmit(event: any) {
    alert('A name was submitted: ' + this.state.value);
    const data = {selectedRSS: this.state.selectedRSS}

    fetch('http://localhost:3306/customrsspodcast', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    })
    .then(res => {
      console.log(res.statusText)
    })


    
    event.preventDefault();
  }

  render() {
    return (
      <ThemeProvider theme={ThemeStandard}>
      <form onSubmit={this.handleSubmit}>
        <label>
          RSS Feed Link: 
          <input id="selectedRSS" type="text" value={this.state.value} onChange={this.handleChange} />
        </label>
        <input type="submit" value="Submit" />
      </form>
      </ThemeProvider>
    );
  }
}

export { RSSForm };
//-----------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
//for getting YouTube input
class YTForm extends React.Component <{}, { value: string }> {
  constructor(props: any) {
    super(props);
    this.state = {value: ''};

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(event: any) {
    this.setState({value: event.target.value});
  }

  handleSubmit(event: any) {
    alert('A name was submitted: ' + this.state.value);
    event.preventDefault();
  }

  render() {
    return (
      <ThemeProvider theme={ThemeStandard}>
      <form onSubmit={this.handleSubmit}>
        <label>
          YouTube Channel Link: 
          <input type="text" value={this.state.value} onChange={this.handleChange} />
        </label>
        <input type="submit" value="Submit" />
      </form>
      </ThemeProvider>
    );
  }
}

export { YTForm };
//-----------------------------------------------------------------------------------------

class AddStreamMenu extends React.Component<{}, { showComponent: boolean }> {
  constructor(props: any) {
    super(props);
    this.state = {
      showComponent: false,
    };
    this._onButtonClick = this._onButtonClick.bind(this);
  }
  _onButtonClick() {
    if(this.state.showComponent === false) {
      this.setState ({
        showComponent: true,
      });
    } else if (this.state.showComponent === true) {
      this.setState ({
        showComponent: false,
      });
    }
  }
    
  render() {
    return (
      <ThemeProvider theme={ThemeStandardBG}>
      <MenuContainer>
        {TabDescriptions.map(({ tab, name, Icon }) => {
          return (
            <div>
            <ThemeProvider theme={ThemeStandard}>
              <MenuButton key={tab} onClick={this._onButtonClick}>
                <IconContainer><Icon /></IconContainer>
                <TextContainer>{name}</TextContainer>
              </MenuButton>
            </ThemeProvider>
              {this.state.showComponent ?
                <FileMenu /> :
                null
              }
              {this.state.showComponent ?
                <RSSMenu /> :
                null
              }
              {this.state.showComponent ?
                <YTMenu /> :
                null
              }
         
            </div>
          );
        })}
      </MenuContainer>
      </ThemeProvider>
    );
  }
};

class FileMenu extends React.Component<{}, { showComponent: boolean }> {
  constructor(props: any) {
    super(props);
    this.state = {
      showComponent: false,
    };
    this._onButtonClick = this._onButtonClick.bind(this);
  }
  _onButtonClick() {
    if(this.state.showComponent === false) {
      this.setState ({
        showComponent: true,
      });
    } else if (this.state.showComponent === true) {
      this.setState ({
        showComponent: false,
      });
    }
  }
  render() {
    return (
      <MenuContainer>
        {FileTab.map(({ tab, name, Icon }) => {
          return (
            <div>
            <ThemeProvider theme={ThemeStandard}>
              <MenuButton key={tab} onClick={this._onButtonClick}>
                <IconContainer><Icon /></IconContainer>
                <TextContainer>{name}</TextContainer>
              </MenuButton>
              {this.state.showComponent ?
                <FileForm /> :
                null
              }
            </ThemeProvider>
            </div>
          );
        })}
      </MenuContainer>
    );
  }
};

class RSSMenu extends React.Component<{}, { showComponent: boolean }> {
  constructor(props: any) {
    super(props);
    this.state = {
      showComponent: false,
    };
    this._onButtonClick = this._onButtonClick.bind(this);
  }
  _onButtonClick() {
    if(this.state.showComponent === false) {
      this.setState ({
        showComponent: true,
      });
    } else if (this.state.showComponent === true) {
      this.setState ({
        showComponent: false,
      });
    }
  }
  render() {
    return (
      <MenuContainer>
        {RSSTab.map(({ tab, name, Icon }) => {
          return (
            <div>
            <ThemeProvider theme={ThemeStandard}>
              <MenuButton key={tab} onClick={this._onButtonClick}>
                <IconContainer><Icon /></IconContainer>
                <TextContainer>{name}</TextContainer>
              </MenuButton>
              {this.state.showComponent ?
                <RSSForm /> :
                null
              }
            </ThemeProvider>
            </div>
          );
        })}
      </MenuContainer>
    );
  }
};

class YTMenu extends React.Component<{}, { showComponent: boolean }> {
  constructor(props: any) {
    super(props);
    this.state = {
      showComponent: false,
    };
    this._onButtonClick = this._onButtonClick.bind(this);
  }
  _onButtonClick() {
    if(this.state.showComponent === false) {
      this.setState ({
        showComponent: true,
      });
    } else if (this.state.showComponent === true) {
      this.setState ({
        showComponent: false,
      });
    }
  }
  render() {
    return (
      <MenuContainer>
        {YouTubeTab.map(({ tab, name, Icon }) => {
          return (
            <div>
              <ThemeProvider theme={ThemeStandard}>
              <MenuButton key={tab} onClick={this._onButtonClick}>
                <IconContainer><Icon /></IconContainer>
                <TextContainer>{name}</TextContainer>
              </MenuButton>
              {this.state.showComponent ?
                <YTForm /> :
                null
              }
            </ThemeProvider>
            </div>
          );
        })}
      </MenuContainer>
    );
  }
};

export default AddStreamMenu;