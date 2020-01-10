# Next Trams in Helsinki

## An Alexa Skill / Azure Function to Access Realtime Tram Data from HSL

An Azure Function built to serve as a backend for an Alexa Skill to utilise [the open timetable and realtime traffic data](https://www.hsl.fi/en/opendata) available from HSL (Helsinki Regional Transport Authority). Made with :heart: and :coffee: in Helsinki.

This project is currently just a proof-of-wconcept working on my own Alexa account and the station codes for testing purposes are hardcoded into the code.

## Motivation

This project gets its inspiration from my own need for a quick way to check the realtime arrival of trams to the stops closest to my home. Even though the official HSL mobile app has a great interface to check realtime arrival times, I really have no time to stop to look at my phone when rushing to leave at home. I wanted to be able to just ask my Amazon Echo for arrival times while running around my apartment looking for my keys, wallet and other essentials.

## Things I've Learnt So Far

In addition to knowing whether I need to run to the tram stop or if I can take my time and sip some more coffee, building the skill has exposed me to a lot of things I did not learn about during my coding bootcamp in 2019: I learnt how to do API calls in C#, how to deploy functions to Azure and how GraphQL works (this was the biggest learning curve as we only covered REST APIs and the HSL traffic data is only available through GraphQL).

## Things To Be Done

- Making it possible to save the user's stop/station preferences through the Alexa app (Alexa does not do very well with Finnish names)
- Adding the necessary data structures to hear timetables of train and metro stations and other 'terminals' in the HSL data
- Improving the pronunciation of station names by using SSML in responses
- Validating the Alexa requests so that the skill could actually be added to the Alexa Skills Store

## Video Demo

**[Click here to view a video of the skill in use, running on an Amazon Echo](https://youtu.be/BOPlA7HEye0)**