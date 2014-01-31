require 'rspec'
require_relative '../lib/poker_hands_app'
require_relative '../lib/player_parser'

describe "PokerHandsApp" do
	context "AcceptanceTests" do
		let(:ph) { PokerHandsApp.new }
		it "generates correct result for high card ace" do
			result = ph.compare_hands("Black: 2H 3D 5S 9C KD White: 2C 3H 4S 8C AH")
			result.should == "White wins - high card: Ace"
		end

		it "generates correct result for flush" do
			result = ph.compare_hands("Black: 2H 3D 5S 9C KD White: 2S 8S AS QS 3S")
			result.should == "White wins - flush"
		end

		it "generates correct result for high card tie" do
			result = ph.compare_hands("Black: 2H 3D 5S 9C KD White: 2C 3H 4S 8C KH")
			result.should == "Black wins - high card: 9"
		end
	end

	context "Input Parsing" do
		context "parsing single player" do
			let(:parser) { PlayerParser.new }
			let(:player) { parser.parse("White: 2C 3H 4S 8C AH") }
			let(:cards) { parser.parse_cards("2C 3H 4S 8C AH") }
			it "should parse player name" do 
				player.name.should == "White"
			end

			it "should parse first cards correctly" do
				expectedCards = [
					Card.new('2C'),
					Card.new('3H'),
					Card.new('4S'),
					Card.new('8C'),
					Card.new('AH')
				]

				cards.should =~ expectedCards
			end

		end
	end

	context "get rank" do
		let(:player) { Player.new }
		it "should recognize flush" do
			player.cards = [
				Card.new('2S'),
				Card.new('8S'),
				Card.new('AS'),
				Card.new('QS'),
				Card.new('3S')
			]
			player.rank.to_s.should == "flush"
		end
	end

	context "comparing ranks" do
		let(:ph) { PokerHandsApp.new }
		it "should recognize highest rank" do
			player1 = Player.new "white"
			player1.cards = [
				Card.new('2C'),
				Card.new('3H'),
				Card.new('4S'),
				Card.new('8C'),
				Card.new('AH')
			]
			player2 = Player.new "black"
			player2.cards = [
				Card.new('2H'),
				Card.new('3D'),
				Card.new('5S'),
				Card.new('9C'),
				Card.new('KD')
			]

			result = ph.determine_result(player1, player2)
			result.winner.should == player1
		end

		it "should recognize low flush higher than high high card" do
			player1 = Player.new "white"
			player1.cards = [
				Card.new('2C'),
				Card.new('3C'),
				Card.new('4C'),
				Card.new('8C'),
				Card.new('5C')
			]
			player2 = Player.new "black"
			player2.cards = [
				Card.new('2H'),
				Card.new('3D'),
				Card.new('5S'),
				Card.new('9C'),
				Card.new('AD')
			]

			result = ph.determine_result(player1, player2)
			result.winner.should == player1
		end
	end
end