export class RoomItem{
    public id: number;
    public creator: string;
    public game: string;
    public maxplayers: number;
    constructor(id: number, creator: string, game: string, maxplayers: number){
        this.id = id;
        this.creator = creator;
        this.game = game;
        this.maxplayers = maxplayers;
    }
}