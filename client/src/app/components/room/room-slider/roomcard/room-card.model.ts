export class RoomCard {
    public lobby: string;
    public description: string;
    public gameImagePath: string;
    public profilePicturePath: string;
    public players: number;
    public maxPlayers: number;
    public topic: string;
    public creator: string;
    public game: string;

    constructor(
        lobby: string,
        description:string,
        gameImagePath: string,
        profilePicturePath: string,
        players: number,
        maxPlayers: number,
        topic: string,
        creator: string,
        game: string) {
        this.lobby = lobby;
        this.description = description;
        this.gameImagePath = gameImagePath;
        this.profilePicturePath = profilePicturePath;
        this.players = players;
        this.maxPlayers = maxPlayers;
        this.topic = topic;
        this.creator = creator;
        this.game = game;
    }


}