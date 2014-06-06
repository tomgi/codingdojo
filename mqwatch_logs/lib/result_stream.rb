class ResultStream
	def initialize
		@lines = []
	end

	def << line
		@lines << line
	end

	def lines
		@lines
	end

	def last_line
		@lines.last || ""
	end
end